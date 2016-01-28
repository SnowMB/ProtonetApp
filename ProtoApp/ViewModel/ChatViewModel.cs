using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProtoApp.ViewModel
{
    public class ChatViewModel: ViewModelBase
    {
        private PrivateChat chat;
        public PrivateChat Chat { get { return chat; } private set { Set(nameof(Chat), ref chat, value); } }
        public ObservableCollection<Meep> Meeps { get; } = new ObservableCollection<Meep>();


        private IProtonetDataService service;

        public ChatViewModel(IProtonetDataService dataService)
        {
            service = dataService;
        }



        public ICommand SendCommand => new RelayCommand<string>(async s => await Send(s));

        private async Task Send(string s)
        {
            try
            {
                var meep = await service.createMeep(Chat.ID, ObjectType.PrivateChat, s);
                Meeps.Add(meep);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }






        public async Task loadChat(int id)
        {
            Chat = await service.getPrivateChat(id);
            Meeps.Clear();
            var meeps = await service.getMeeps(id, ObjectType.PrivateChat);
            foreach (var m in meeps)
                Meeps.Add(m);
            
        }
    }
}
