using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

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
                Default.Register<IProtonetDataService, DesignDataService>();
            }
            else
            {
                Default.Register<IProtonetImages>(() => new ProtonetImages(DataClient));
                //Default.Register<IProtonetClient>(() => new ProtonetClient("https://192.168.11.2/"));
                Default.Register<IProtonetClient>(() => new ProtonetClient("https://stier74.protonet.info/"));
            }

            //ViewModels
            Default.Register<MainViewModel>();
            Default.Register<ChatsViewModel>();
            Default.Register<ChatViewModel>();
            Default.Register<LoginViewModel>();

            //Navigation
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
            Default.Register<IDialogService>(() => new DialogService());


            DataClient.AuthentificationFailed += (s, e) => NavigationService.NavigateTo("Login");
            DataClient.AuthentificationComplete += (s, e) => NavigationService.NavigateTo("Main");
            DataClient.LoggedOut += (s, e) => NavigationService.NavigateTo("Login");

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
