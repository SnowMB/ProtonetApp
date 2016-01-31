using ProtoApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ProtoApp
{
    public class ViewModelPage : Page
    {
        public NavigatedViewModel ViewModel => DataContext as NavigatedViewModel;


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if (ViewModel != null)
                ViewModel.OnNavigatedTo(e.Parameter);

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (ViewModel != null)
                ViewModel.OnNavigatedFrom(e.Parameter);

        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            
            if (ViewModel != null)
                ViewModel.OnNavigatingFrom(e.Parameter);
        }
    }
}
