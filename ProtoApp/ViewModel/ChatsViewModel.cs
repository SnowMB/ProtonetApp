using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProtoApp.ViewModel
{
  public class ChatsViewModel : NavigatedViewModel
  {
    public ObservableCollection<PrivateChat> Chats { get; set; } = new ObservableCollection<PrivateChat> ();

    private IProtonetClient client;
    private IDialogService dialog;
    private INavigationService navigation;

    public ICommand NavigateChatCommand => new RelayCommand<string> ( url => navigation.NavigateTo ( "Chat", url ) );


    public ChatsViewModel ( IProtonetClient protoClient, IDialogService dialogService, INavigationService navigationService )
    {
      client = protoClient;
      dialog = dialogService;
      navigation = navigationService;
    }


    public async Task loadChats ()
    {
      Chats.Clear ();
      var newChats = await client.GetChatsAsync(client.User.PrivateChatsUrl);
      foreach ( var chat in newChats )
        Chats.Add ( chat );
    }

    public async override void OnNavigatedTo ( object param )
    {
      base.OnNavigatedTo ( param );

      try
      {
        await loadChats ();
      }
      catch ( Exception ex )
      {
        await dialog.ShowMessage ( ex.ToString (), "Error" );
        navigation.GoBack ();
      }
    }

  }
}
