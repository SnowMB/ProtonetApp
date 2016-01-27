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
    public sealed partial class LoginPage : Page
    {
        public LoginViewModel ViewModel => DataContext as LoginViewModel;

        public LoginPage()
        {
            InitializeComponent();

            ViewModel.LoginSucessfull += ViewModel_LoginSucessfull;
            ViewModel.LoginFailed += ViewModel_LoginFailed;
        }

        private async void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            var dialog = new MessageDialog("Login failed!");
            await dialog.ShowAsync();
        }

        private void ViewModel_LoginSucessfull(object sender, EventArgs e)
        {
            Frame.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
