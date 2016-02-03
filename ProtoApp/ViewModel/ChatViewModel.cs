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
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

namespace ProtoApp.ViewModel
{
    public class ChatViewModel: NavigatedViewModel
    {
        private PrivateChat chat;
        public PrivateChat Chat { get { return chat; } private set { Set(nameof(Chat), ref chat, value); } }
        public ObservableCollection<FileMeep> Meeps { get; } = new ObservableCollection<FileMeep>();



        






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
                Meeps.Add(CreateFileMeep(meep));
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
                    Meeps.Add(CreateFileMeep(meep));
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
                var filemeep = await GetLocalFiles(m);
                Meeps.Add(filemeep);   
            }
        }

        private async Task<FileMeep> GetLocalFiles(Meep m)
        {
            var fileTasks = new List<Task>();


            var fileMeep = CreateFileMeep(m);


            foreach (var f in m.Files)
            {
                var task  = iservice.GetOrDownloadThumbnailFile(f.ThumbnailUrl, f.ID, f.Type);
                var fullTask = task.ContinueWith(async x => fileMeep.LocalFiles.Add(await (await task).OpenReadAsync()));
  
                fileTasks.Add(fullTask);
            }

            await Task.WhenAll(fileTasks.ToArray());

            return fileMeep;
        }




        private FileMeep CreateFileMeep(Meep m)
        {
            return new FileMeep()
            {
                CreatedAt = m.CreatedAt,
                Files = m.Files,
                ID = m.ID,
                Message = m.Message,
                Number = m.Number,
                Type = m.Type,
                UpdatedAt = m.UpdatedAt,
                Url = m.Url,
                User = m.User
            };
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
