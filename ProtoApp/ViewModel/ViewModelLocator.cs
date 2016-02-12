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
                //Default.Register<IProtonetClient>(() => new ProtonetClient("https://192.168.11.2/"));
                Default.Register<IProtonetClient>(() => new ProtonetClient("https://stier74.protonet.info/"));
            }

            //ViewModels
            Default.Register<MainViewModel>();
            Default.Register<ChatsViewModel>();
            Default.Register<ChatViewModel>();
            Default.Register<LoginViewModel>();

            //Navigation
            Default.Register<INavigationService>(() => new NavigationService());


            var nav = NavigationService as NavigationService;
            nav.Configure("Login", typeof(LoginPage));
            nav.Configure("Main", typeof(MainPage));
            nav.Configure("Chats", typeof(ChatsPage));
            nav.Configure("Chat", typeof(ChatPage));

            //DialogService
            Default.Register<IDialogService>(() => new DialogService());


            DataClient.AuthentificationFailed += LoggedOut;
            DataClient.LoggedOut += LoggedOut;

            DataClient.AuthentificationComplete += LoggedIn;
        }









        private void LoggedIn(object sender, EventArgs e)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential("ProtonetApp", DataClient.User.UserName, DataClient.Token));

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["server"] = DataClient.User.Url;

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
