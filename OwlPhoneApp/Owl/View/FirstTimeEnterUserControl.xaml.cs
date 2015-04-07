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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OwlWindowsPhoneApp.View
{
    public sealed partial class FirstTimeEnterUserControl : UserControl
    {
        public event EventHandler<EventArgs> GuideFinished;
        public event EventHandler<EventArgs> TakePhotoClick;
        private bool _profileChoosed = false;

        public FirstTimeEnterUserControl()
        {
            this.InitializeComponent();
        }

        private async void AppBarButton_Next_Click(object sender, RoutedEventArgs e)
        {
            if (FlipView_Profile.SelectedIndex == 0)
            {
                if(_profileChoosed == true)
                {
                    FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex + 1;
                }
                else
                {
                    var dialog = new MessageDialog("Pleas pick a profile photo");
                    dialog.Commands.Add(new UICommand("OK"));
                    await dialog.ShowAsync();
                }
            }
            else
            {
                if (FlipView_Profile.SelectedIndex == FlipView_Profile.Items.Count - 1)
                {
                    if (GuideFinished != null)
                        GuideFinished(this, null);
                }
                else
                    FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex + 1;
            }
        }

        private void Image_Profile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(TakePhotoClick != null)
            {
                TakePhotoClick(this, null);
            }
        }

        public void ChangeImageProfile(RenderTargetBitmap bmp)
        {
            Image_Profile.Source = bmp;
            if (bmp != null)
                _profileChoosed = true;
        }

        private void AppBarButton_Back_Click(object sender, RoutedEventArgs e)
        {
            if (FlipView_Profile.SelectedIndex > 0)
            {
                FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex - 1;
            }
        }
    }
}
