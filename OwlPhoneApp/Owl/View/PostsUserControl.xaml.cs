using GalaSoft.MvvmLight.Messaging;
using Owl.DataObjects;
using Owl.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.Data.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Owl
{
    public sealed partial class PostsUserControl : UserControl
    {   
        public PostsUserControl()
        {
            this.InitializeComponent();
            this.Loaded += PostsUserControl_Loaded;
            Messenger.Default.Register<LoadingAnimationMessage>(this, async msg =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (msg.IfLoading)
                    {
                        ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        ProgressBar_Loading.IsIndeterminate = true;
                    }
                    else
                    {
                        ProgressBar_Loading.IsIndeterminate = false;
                        ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                });
            });
        }

        void PostsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
