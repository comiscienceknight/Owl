using GalaSoft.MvvmLight.Messaging;
using OwlWindowsPhoneApp.ViewModel;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OwlWindowsPhoneApp
{
    public sealed partial class ChatHistoryUserControl : UserControl
    {
        public ChatHistoryUserControl()
        {
            this.InitializeComponent();
            this.Loaded += PostsUserControl_Loaded;
            Messenger.Default.Register<LoadingAnimationMessage>(this, LoadingAnimationMessage.ChatToken, async msg =>
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
