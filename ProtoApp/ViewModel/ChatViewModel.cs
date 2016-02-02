using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ProtoApp.ViewModel
{
    public class ChatViewModel: NavigatedViewModel
    {
        private PrivateChat chat;
        public PrivateChat Chat { get { return chat; } private set { Set(nameof(Chat), ref chat, value); } }
        public ObservableCollection<Meep> Meeps { get; } = new ObservableCollection<Meep>();



        






        private IProtonetClient client;
        private IProtonetImages iservice;
        private IDialogService dialoges;
        private INavigationService navigation;

        public ChatViewModel(IProtonetClient protoClient, IProtonetImages imagesService, IDialogService dialogService, INavigationService navigationService )
        {
            client = protoClient;
            iservice = imagesService;
            dialoges = dialogService;
            navigation = navigationService;
        }


        private ICommand sendCommand;
        public ICommand SendCommand => sendCommand != null? sendCommand : sendCommand = new RelayCommand<string>(async s => await Send(s), s => string.IsNullOrWhiteSpace(s));
        private async Task Send(string s)
        {
            try
            {
                var meep = await client.CreateMeep(Chat.MeepsUrl, new NewMeep() { Message = s });
                Meeps.Add(meep);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public ICommand SendFileCommand => new RelayCommand(SendFile);
        private async void SendFile()
        {
            try
            {
                var filePicker = new FileOpenPicker();
                filePicker.FileTypeFilter.Add(".jpg");
                var file = await filePicker.PickSingleFileAsync();
                if (file != null)
                {
                    var read = await file.OpenReadAsync().AsTask();
                    var meep = await client.CreateFileMeep(chat.MeepsUrl, read.AsStreamForRead());
                    Meeps.Add(meep);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }




        public async Task loadChat(string url)
        {
            Chat = await client.GetChat(url);
            Meeps.Clear();
            var meeps = await client.GetChatMeeps(Chat.MeepsUrl);
            foreach (var m in meeps)
            {
                await GetLocalImages(m);
                Meeps.Add(m);
            }
        }

        private async Task GetLocalImages(Meep m)
        {
            var imageTasks = new List<Task>();

            foreach (var f in m.Files)
            {
                var task  = iservice.GetLocalOrDownloadImage(f.ThumbnailUrl, f.ID);
                var fullTask = task.ContinueWith( x =>
                {
                    f.ThumbnailUrl = x.Result.Path;
                    return;
                });
                imageTasks.Add(fullTask);
            }

            await Task.WhenAll(imageTasks.ToArray());
        }



        public async override void OnNavigatedTo(object param)
        {
            base.OnNavigatedTo(param);

            try
            {
                var url = (string)param;
                await loadChat(url);
            }
            catch (Exception ex)
            {
                await dialoges.ShowMessage(ex.ToString(), "");

                navigation.GoBack();
            }
        }
    }
}
