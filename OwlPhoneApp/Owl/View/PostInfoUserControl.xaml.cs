using OwlWindowsPhoneApp.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OwlWindowsPhoneApp
{
    public sealed partial class PostInfoUserControl : UserControl
    {
        private readonly Post _post;

        public PostInfoUserControl()
            :this(null)
        {

        }

        public PostInfoUserControl(Post post)
        {
            this.InitializeComponent();
            _post = post;
            this.Loaded += PostInfoUserControl_Loaded;
        }

        void PostInfoUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_post != null)
            {
                try
                {
                    TextBlock_PageTitle.Text = "Owl, I am " + (_post.UserName ?? "");

                    FlipView_ProfileBackground.Width = Window.Current.Bounds.Width - 30;
                    FlipView_ProfileBackground.Height = FlipView_ProfileBackground.Width * 0.75;
                    FlipView_Profile.Width = FlipView_ProfileBackground.Width + 4;
                    FlipView_Profile.Height = FlipView_ProfileBackground.Height + 3;
                    FlipView_NightClubPhoto.Width = FlipView_ProfileBackground.Width + 4;
                    FlipView_NightClubPhoto.Height = FlipView_ProfileBackground.Height + 3;

                    if (!string.IsNullOrWhiteSpace(_post.ProfileUrl) && _post.ProfileUrl.Length > 4)
                    {
                        ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Visible;

                        var bitmap = new BitmapImage();
                        bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                        bitmap.UriSource = new Uri(_post.ProfileUrl, UriKind.Absolute);
                        Image_Profile1.Source = bitmap;
                    }

                    TextBlock_Require.Text = _post.Require ?? "";
                    TextBlock_Description.Text = _post.Description ?? "";
                    TextBlock_ClubTitle.Text = _post.Place ?? "";
                    TextBlock_PlaceAddresse.Text = _post.PlaceAddresse ?? "";
                }
                catch(Exception exp)
                {

                }
            }
        }

        public Post GetPost()
        {
            return _post;
        }

        private void Image_Profile1_ImageOpened(object sender, RoutedEventArgs e)
        {
            ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            FlipView_ProfileBackground.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
