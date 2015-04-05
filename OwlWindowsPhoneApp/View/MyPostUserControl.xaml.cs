using GalaSoft.MvvmLight.Messaging;
using OwlWindowsPhoneApp.ViewModel.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
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
    public sealed partial class MyPostUserControl : UserControl
    {
        public MyPostUserControl()
        {
            this.InitializeComponent();
            this.Loaded += MyPostUserControl_Loaded;
        }

        void MyPostUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FlipView_Profile.Width = Window.Current.Bounds.Width - 10;
            FlipView_Profile.Height = FlipView_Profile.Width * 0.75;
        }

        private void Image_Profile1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Messenger.Default.Send<NavigateToCameraMessage>(new NavigateToCameraMessage());
        }

        private void Image_Profile2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Messenger.Default.Send<NavigateToCameraMessage>(new NavigateToCameraMessage());
        }

        private void Image_Profile3_Tapped(object sender, TappedRoutedEventArgs e)
        {
    Messenger.Default.Send<NavigateToCameraMessage>(new NavigateToCameraMessage());
        }
    }
}
