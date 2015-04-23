using Owl.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
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
using Windows.Web.Http;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Owl
{
    public sealed partial class PostInfoUserControl : UserControl
    {
        private Post _post = null;

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
                Load();
            }
        }

        public void Load(bool isCurrentUser = false)
        {
            try
            {
                int horizontalOffset = 40;
                if (isCurrentUser == false)
                    TextBlock_PageTitle.Text = "Owl, I am " + (_post.UserName ?? "");
                else
                {
                    TextBlock_PageTitle.Text = (_post.UserName ?? "");
                    Image_Title.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Grid_ContentRoot.Margin = new Thickness(5, 70, 10, 0);
                    Rectangle_Title.Margin = new Thickness(5, 50, 10, 0);
                    TextBlock_PageTitle.Margin = new Thickness(0, 6, 0, 0);
                    TextBlock_PageTitle.FontSize = 28;
                    TextBlock_ClubTitle.FontSize = 28;
                    TextBlock_PageTitle.Opacity = 1;
                    horizontalOffset = 55;
                }

                ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Visible;

                Rectangle_Title.Width = Window.Current.Bounds.Width - 100;
                Rectangle_Club.Width = Window.Current.Bounds.Width - 100;
                FlipView_ProfileBackground.Width = Window.Current.Bounds.Width - horizontalOffset;
                FlipView_ProfileBackground.Height = FlipView_ProfileBackground.Width * 0.9;
                FlipView_Profile.Width = FlipView_ProfileBackground.Width + 4;
                FlipView_Profile.Height = FlipView_ProfileBackground.Height + 3;
                FlipView_NightClubPhoto.Width = FlipView_ProfileBackground.Width + 4;
                FlipView_NightClubPhoto.Height = FlipView_ProfileBackground.Width * 0.8 + 3;

                if (!string.IsNullOrWhiteSpace(_post.ProfileUrl) && _post.ProfileUrl.Length > 4)
                {
                    var bitmap1 = new BitmapImage();
                    bitmap1.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap1.UriSource = new Uri(_post.ProfileUrl, UriKind.Absolute);
                    Image_Profile1.Source = bitmap1;
                }
                if (!string.IsNullOrWhiteSpace(_post.ProfileUrl2))
                {
                    var bitmap2 = new BitmapImage();
                    bitmap2.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap2.UriSource = new Uri(_post.ProfileUrl2, UriKind.Absolute);
                    Image_Profile2.Source = bitmap2;
                }
                if (!string.IsNullOrWhiteSpace(_post.ProfileUrl3))
                {
                    var bitmap3 = new BitmapImage();
                    bitmap3.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap3.UriSource = new Uri(_post.ProfileUrl3, UriKind.Absolute);
                    Image_Profile3.Source = bitmap3;
                }

                if (!string.IsNullOrWhiteSpace(_post.VenuePhotoUrl1))
                {
                    var bitmap1 = new BitmapImage();
                    bitmap1.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap1.UriSource = new Uri(_post.VenuePhotoUrl1, UriKind.Absolute);
                    Image_NightClub1.Source = bitmap1;
                }
                if (!string.IsNullOrWhiteSpace(_post.VenuePhotoUrl2))
                {
                    var bitmap2 = new BitmapImage();
                    bitmap2.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap2.UriSource = new Uri(_post.VenuePhotoUrl2, UriKind.Absolute);
                    Image_NightClub2.Source = bitmap2;
                }
                if (!string.IsNullOrWhiteSpace(_post.VenuePhotoUrl3))
                {
                    var bitmap3 = new BitmapImage();
                    bitmap3.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmap3.UriSource = new Uri(_post.VenuePhotoUrl3, UriKind.Absolute);
                    Image_NightClub3.Source = bitmap3;
                }

                TextBlock_Require.Text = "+ " + (_post.GirlsNumber ?? 0).ToString() + " girls; + " + (_post.GuysNumber ?? 0).ToString() + " boys";
                TextBlock_Description.Text = _post.OtherInfo ?? "";
                TextBlock_ClubTitle.Text = _post.Place ?? "";
                TextBlock_PlaceAddresse.Text = _post.PlaceAddresse ?? "";
                TextBlock_ArrivalTime.Text = "We'll be there at " + _post.ArrivalTime;
                TextBlock_ClubPopularity.Text = _post.VenuePopularity.ToString() + " users have been here";
            }
            catch (Exception)
            {

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
