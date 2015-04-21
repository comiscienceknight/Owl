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

        public async void SetPostByUserId()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                var posts = await httpClient.GetStringAsync(
                    new Uri("http://owlbat.azure-mobile.net/get/getpost/" + App.UserId));

                JsonValue jsonValue = JsonValue.Parse(posts);
                _post = AnalysePostJsonValueArray(jsonValue);
            }
            Load(true);
        }

        private DataObjects.Post AnalysePostJsonValueArray(JsonValue postsJasonValue)
        {
            return AnalysePostJsonValue(postsJasonValue.GetObject());
        }

        private DataObjects.Post AnalysePostJsonValue(IJsonValue postJasonValue)
        {
            JsonObject jo = postJasonValue.GetObject();
            var post = new DataObjects.Post();

            post.Place = jo.GetNamedString("venueName");

            if (jo.ContainsKey("description"))
                post.OtherInfo = jo.GetNamedString("description");

            if (jo.ContainsKey("userProfileUrl1"))
            {
                post.ProfileUrl = jo.GetNamedString("userProfileUrl1");
                if (!string.IsNullOrWhiteSpace(post.ProfileUrl))
                {
                    Uri myUri = new Uri(post.ProfileUrl + "?Width=175", UriKind.Absolute);
                    BitmapImage bmi = new BitmapImage();
                    bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmi.UriSource = myUri;
                    post.Profile = bmi;
                }
            }
            if (jo.ContainsKey("userProfileUrl2"))
            {
                post.ProfileUrl2 = jo.GetNamedString("userProfileUrl2");
                if (!string.IsNullOrWhiteSpace(post.ProfileUrl2))
                {
                    Uri myUri = new Uri(post.ProfileUrl2 + "?Width=175", UriKind.Absolute);
                    BitmapImage bmi = new BitmapImage();
                    bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmi.UriSource = myUri;
                    post.Profile2 = bmi;
                }
            }
            if (jo.ContainsKey("userProfileUrl3"))
            {
                post.ProfileUrl3 = jo.GetNamedString("userProfileUrl3");
                if (!string.IsNullOrWhiteSpace(post.ProfileUrl3))
                {
                    Uri myUri = new Uri(post.ProfileUrl3 + "?Width=175", UriKind.Absolute);
                    BitmapImage bmi = new BitmapImage();
                    bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmi.UriSource = myUri;
                    post.Profile3 = bmi;
                }
            }

            if (jo.ContainsKey("placeAddresse"))
            {
                post.PlaceAddresse = jo.GetNamedString("placeAddresse");
            }

            if (jo.ContainsKey("venuePosition"))
            {
                post.VenuePosition = jo.GetNamedString("venuePosition");
            }

            if (jo.ContainsKey("venuePopularity"))
            {
                post.VenuePopularity = (int)jo.GetNamedNumber("venuePopularity");
            }

            if (jo.ContainsKey("venuePhotoUrl1"))
            {
                post.VenuePhotoUrl1 = jo.GetNamedString("venuePhotoUrl1");
            }
            if (jo.ContainsKey("venuePhotoUrl2"))
            {
                post.VenuePhotoUrl2 = jo.GetNamedString("venuePhotoUrl2");
            }
            if (jo.ContainsKey("venuePhotoUrl3"))
            {
                post.VenuePhotoUrl3 = jo.GetNamedString("venuePhotoUrl3");
            }

            if (jo.ContainsKey("girlNumber"))
                post.GirlsNumber = (int)jo.GetNamedNumber("girlNumber");

            if (jo.ContainsKey("boyNumber"))
                post.GuysNumber = (int)jo.GetNamedNumber("boyNumber");

            if (jo.ContainsKey("userName"))
                post.UserName = jo.GetNamedString("userName");

            if (jo.ContainsKey("time"))
                post.Time = jo.GetNamedString("time");

            if (jo.ContainsKey("venueId"))
                post.VenueId = jo.GetNamedString("venueId");

            if (jo.ContainsKey("userPopularity"))
                post.UserPopularity = (int)jo.GetNamedNumber("userPopularity");

            post.UserId = jo.GetNamedString("userId");

            post.Require = string.Format("+ {0} boys, + {1} girls. {2}", post.GuysNumber, post.GirlsNumber, post.Time);

            return post;
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

                TextBlock_Require.Text = _post.Require ?? "";
                TextBlock_Description.Text = _post.OtherInfo ?? "";
                TextBlock_ClubTitle.Text = _post.Place ?? "";
                TextBlock_PlaceAddresse.Text = _post.PlaceAddresse ?? "";
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
