﻿using GalaSoft.MvvmLight.Messaging;
using Owl.ViewModel.Message;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Owl
{
    public sealed partial class MyPostUserControl : UserControl
    {
        public event EventHandler<ProfilePhotoClickEventArg> TakePhotoEvent;

        public MyPostUserControl()
        {
            this.InitializeComponent();
            this.Loaded += MyPostUserControl_Loaded;
        }

        void MyPostUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FlipView_Profile.Width = Window.Current.Bounds.Width - 30;
            FlipView_Profile.Height = FlipView_Profile.Width * 0.9;
            Image_Profile1.Width = FlipView_Profile.Width - 2;
            Image_Profile1.Height = FlipView_Profile.Height - 2;
            Image_Profile2.Width = FlipView_Profile.Width - 2;
            Image_Profile2.Height = FlipView_Profile.Height - 2;
            Image_Profile3.Width = FlipView_Profile.Width - 2;
            Image_Profile3.Height = FlipView_Profile.Height - 2;
        }

        private void Image_Profile1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (TakePhotoEvent != null)
                TakePhotoEvent(this, new ProfilePhotoClickEventArg()
                {
                    ProfilePhotoNumber = 0
                });
            //Messenger.Default.Send<NavigateToCameraMessage>(new NavigateToCameraMessage(0));
        }

        private void Image_Profile2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (TakePhotoEvent != null)
                TakePhotoEvent(this, new ProfilePhotoClickEventArg()
                {
                    ProfilePhotoNumber = 1
                });
            //Messenger.Default.Send<NavigateToCameraMessage>(new NavigateToCameraMessage(1));
        }

        private void Image_Profile3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (TakePhotoEvent != null)
                TakePhotoEvent(this, new ProfilePhotoClickEventArg()
                {
                    ProfilePhotoNumber = 2
                });
            //Messenger.Default.Send<NavigateToCameraMessage>(new NavigateToCameraMessage(2));
        }

        public void ChangeImageProfile(RenderTargetBitmap bmp, int profileNumber)
        {
            switch(profileNumber)
            {
                case 0:
                    Image_Profile1.Source = bmp;
                    break;
                case 1:
                    Image_Profile2.Source = bmp;
                    break;
                case 2:
                    Image_Profile3.Source = bmp;
                    break;
            }
        }
    }

    public class ProfilePhotoClickEventArg : EventArgs
    {
        public int ProfilePhotoNumber {get;set;}
    }
}
