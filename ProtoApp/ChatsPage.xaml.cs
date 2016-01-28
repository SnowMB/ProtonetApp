using ProtoApp.Objects;
using ProtoApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace ProtoApp
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class ChatsPage : Page
    {
        public ChatsViewModel ViewModel => DataContext as ChatsViewModel;

        public ChatsPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
             base.OnNavigatedTo(e);

            try
            {
                await ViewModel.loadChats();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.ToString());
                await dialog.ShowAsync();
                Frame.GoBack();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var chat = btn.DataContext as PrivateChat;

            Frame.Navigate(typeof(ChatPage), chat.ID);
        }
    }
}
