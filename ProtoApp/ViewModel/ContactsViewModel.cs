using GalaSoft.MvvmLight.Views;
using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoApp.ViewModel
{
    public class ContactsViewModel : NavigatedViewModel
    {

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();


        private IProtonetClient client;
        private IDialogService dialog;
        private INavigationService navigation;

        public ContactsViewModel(IProtonetClient client, IDialogService dialog, INavigationService navigation)
        {
            this.client = client;
            this.dialog = dialog;
            this.navigation = navigation;
        }



        public override async void OnNavigatedTo(object param)
        {
            base.OnNavigatedTo(param);
            Users.Clear();

            try
            {
                await LoadContacts();
            }
            catch (Exception ex)
            {
                await dialog.ShowMessage(ex.ToString(), "Error");
                navigation.GoBack();
            } 
        }


        private async Task LoadContacts ()
        {
            var newUser = await client.GetUsersAsync(client.UsersUrl);

            foreach (var u in newUser)
            {
                Users.Add(u);
            }
        }

    }
}
