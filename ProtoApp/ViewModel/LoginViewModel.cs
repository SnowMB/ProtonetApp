using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProtoApp.ViewModel
{
    public class LoginViewModel : NavigatedViewModel
    {
        public string Name { get; set; } = "";

        public string Server { get; set; } = "";


        public ICommand LoginCommand => new RelayCommand<string>(async s => await LoginAsync(s));

        private IProtonetClient client;
        private INavigationService navigation;
    private IDialogService dialog;

    public LoginViewModel(IProtonetClient protoClient, INavigationService navigationService, IDialogService dialogService)
        {
            client = protoClient;
            navigation = navigationService;
      dialog = dialogService;

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Server = localSettings.Values["server"] as string;
        }

        private async Task LoginAsync(string s)
        {
      try
      {
        await client.AuthentificateAsync ( Server, Name, s );
      }
      catch(Exception ex)
      {
        await dialog.ShowMessage ( ex.ToString (), "Error" );
      }
        
        }


        public override void OnNavigatedTo(object param)
        {
            if (client.User != null)
                Name = client.User.UserName;
        }

    }
}
