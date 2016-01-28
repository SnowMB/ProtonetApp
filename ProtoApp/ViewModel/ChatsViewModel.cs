using GalaSoft.MvvmLight;
using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoApp.ViewModel
{
    public class ChatsViewModel : ViewModelBase
    {
        public ObservableCollection<PrivateChat> Chats { get; set; } = new ObservableCollection<PrivateChat>();

        private IProtonetDataService service;

        public ChatsViewModel(IProtonetDataService dataService)
        {
            service = dataService;
        }


        public async Task loadChats()
        {
            Chats.Clear();
            var newChats = await service.getPrivateChats();
            foreach(var chat in newChats)
                Chats.Add(chat);
        }

    }
}
