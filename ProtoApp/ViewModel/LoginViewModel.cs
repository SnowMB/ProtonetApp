using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProtoApp.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public string Name { get; set; }

        //public string Password { get; set; }


        public event EventHandler LoginSucessfull;
        public void OnLoginSucessfull() => LoginSucessfull?.Invoke(this, EventArgs.Empty);

        public event EventHandler LoginFailed;
        public void OnLoginFailed() => LoginFailed?.Invoke(this, EventArgs.Empty);



        public ICommand LoginCommand => new RelayCommand<string>(async s => await Login(s));

        private IProtonetDataService service;

        public LoginViewModel(IProtonetDataService dataService)
        {
            service = dataService;
        }


        
        private async Task Login(string s)
        {
            try
            {
                var token = await service.getToken(Name, s);
                if (!string.IsNullOrWhiteSpace(token?.Token))
                {
                    service.Token = token.Token;
                    OnLoginSucessfull();
                }
            }
            catch
            {
                OnLoginFailed();
            }
            
        }


    }
}
