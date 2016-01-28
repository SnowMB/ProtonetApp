using ProtoApp.ViewModel;
using ProtoApp.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace ProtoApp
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class ChatPage : Page
    {
        public ChatViewModel ViewModel => this.DataContext as ChatViewModel;

        public ChatPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                var chatID = (int)e.Parameter;
                await ViewModel.loadChat(chatID);
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.ToString());
                await dialog.ShowAsync();
                Frame.GoBack();
            }

        }
    }
}
