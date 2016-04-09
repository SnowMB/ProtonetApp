using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Windows.Security.Credentials;
using static GalaSoft.MvvmLight.Ioc.SimpleIoc;

namespace ProtoApp.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                Default.Register<IProtonetClient, DummyClient>();
                Default.Register<IProtonetImages, ProtonetImages>();
            }
            else
            {
                Default.Register<IProtonetImages, ProtonetImages>();
                Default.Register<IProtonetClient, ProtonetClient>();
                //Default.Register<IProtonetClient>(() => new ProtonetClient("https://192.168.11.2/"));

            }

            //ViewModels
            Default.Register<MainViewModel>();
            Default.Register<ChatsViewModel>();
            Default.Register<ChatViewModel>();
            Default.Register<LoginViewModel>();

            //Navigation
            Default.Unregister<INavigationService>(); //Unregister to prevent "already registered"- Errors in design time.
            Default.Register<INavigationService>(() =>
            {
                var nav = new NavigationService();
                nav.Configure("Login", typeof(LoginPage));
                nav.Configure("Main", typeof(MainPage));
                nav.Configure("Chats", typeof(ChatsPage));
                nav.Configure("Chat", typeof(ChatPage));
                return nav;
            });

            

            //DialogService
            Default.Register<IDialogService, DialogService>();


            DataClient.AuthentificationFailed += LoggedOut;
            DataClient.LoggedOut += LoggedOut;

            DataClient.AuthentificationComplete += LoggedIn;
        }









        private void LoggedIn(object sender, EventArgs e)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential("ProtonetApp", DataClient.User.UserName, DataClient.Token));

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["server"] = DataClient.Server;

            DataClient.CreatePushNotificationChannel("");

            NavigationService.NavigateTo("Main");
        }

        private void LoggedOut(object sender, System.EventArgs e)
        {
            var vault = new PasswordVault();
            var cred = vault.FindAllByResource("ProtonetApp");
            foreach (var c in cred)
                vault.Remove(c);

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.DeleteContainer("server");

            NavigationService.NavigateTo("Login");
        }












        public MainViewModel Main => Default.GetInstance<MainViewModel>();


        public ChatsViewModel Chats => Default.GetInstance<ChatsViewModel>();


        public ChatViewModel Chat => Default.GetInstance<ChatViewModel>();
        

        public LoginViewModel Login => Default.GetInstance<LoginViewModel>();



        public IProtonetClient DataClient => Default.GetInstance<IProtonetClient>();

        public IProtonetImages ImagesService => Default.GetInstance<IProtonetImages>();
        
        public INavigationService NavigationService => Default.GetInstance<INavigationService>();
    }
}
