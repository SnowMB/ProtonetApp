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
    public class ChatViewModel: ViewModelBase
    {
        public ObservableCollection<Meep> Meeps { get; } = new ObservableCollection<Meep>();


        private IProtonetDataService service;

        public ChatViewModel(IProtonetDataService dataService)
        {
            service = dataService;
        }


        public async Task loadChat(int id)
        {
            Meeps.Clear();
            var meeps = await service.getMeeps(id, ObjectType.PrivateChat);
            foreach (var m in meeps.Messages)
                Meeps.Add(m);
            
        }
    }
}
