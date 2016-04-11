using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProtoApp.ViewModel
{
    public class MainViewModel : NavigatedViewModel
    {
        private IProtonetClient client;

        private INavigationService navigation;

        public MainViewModel(IProtonetClient protoClient, INavigationService navigationService)
        {
            client = protoClient;
            navigation = navigationService;

            protoClient.PropertyChanged += DataService_PropertyChanged;
        }

        private void DataService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            navigateChatsCommand.RaiseCanExecuteChanged();
            logoutCommand.RaiseCanExecuteChanged();
        }

        private RelayCommand navigateChatsCommand;
        public ICommand NavigateChatsCommand => navigateChatsCommand != null ? navigateChatsCommand : navigateChatsCommand = new RelayCommand(() => navigation.NavigateTo("Chats"), () => client.IsAuthentificated);

        private RelayCommand logoutCommand;
        public ICommand LogoutCommand => logoutCommand != null ? logoutCommand : logoutCommand = new RelayCommand(() => client.Logout(), () => client.IsAuthentificated);

        private RelayCommand newChatCommand;
        public ICommand NewChatCommand => newChatCommand != null ? newChatCommand : newChatCommand = new RelayCommand(() => navigation.NavigateTo("Contacts"), () => client.IsAuthentificated);
    }
}
