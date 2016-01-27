using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static GalaSoft.MvvmLight.Ioc.SimpleIoc;

namespace ProtoApp.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                Default.Register<IProtonetDataService, DesignDataService>();
            }
            else
            {
                Default.Register<IProtonetDataService, DesignDataService>();
                //Default.Register<IProtonetDataService, ProtonetDataService>();
            }

            Default.Register<MainViewModel>();
            Default.Register<ChatsViewModel>();
            Default.Register<ChatViewModel>();
            Default.Register<LoginViewModel>();
        }

        public MainViewModel Main
        {
            get { return Default.GetInstance<MainViewModel>(); }
        }

        public ChatsViewModel Chats
        {
            get { return Default.GetInstance<ChatsViewModel>(); }
        }

        public ChatViewModel Chat
        {
            get { return Default.GetInstance<ChatViewModel>(); }
        }

        public LoginViewModel Login
        {
            get { return Default.GetInstance<LoginViewModel>(); }
        }
    }
}
