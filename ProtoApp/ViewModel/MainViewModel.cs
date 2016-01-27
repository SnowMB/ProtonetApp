using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public bool Authentificated { get { return !string.IsNullOrWhiteSpace(service.Token); } }

        private IProtonetDataService service;

        public MainViewModel(IProtonetDataService dataService)
        {
            service = dataService;
        }

    }
}
