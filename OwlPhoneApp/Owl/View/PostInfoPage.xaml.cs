using Owl.DataObjects;
using Owl.Models;
using Owl.View.FirstVisit;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Owl
{
    public sealed partial class PostInfoPage : Page
    {
        private Post _post = null;
        private bool _previewMode = false;
        private StorageFile _renderBitmapStorageFile;

        public PostInfoPage()
        {
            this.InitializeComponent();
            this.Loaded += PostInfoUserControl_Loaded;
        }

        void PostInfoUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_post != null)
            {
                Load();
                if (_post.UserId == App.MyPost.UserId)
                {
                    AppBarButton_Edit.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    AppBarButton_Edit.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                StartPreviewMode();
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter is Post)
            {
                _post = e.Parameter as Post;
            }
            else if (e.Parameter is PostInfoNavigationParameter)
            {
                var pp = e.Parameter as PostInfoNavigationParameter;
                if (pp != null)
                {
                    _previewMode = pp.PreviewMode;
                    _post = pp.Post;
                    _renderBitmapStorageFile = pp.RenderBitmapStorageFile;
                }
            }
        }

        public async void Load(bool isCurrentUser = false)
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

                if (_renderBitmapStorageFile != null)
                {
                    Image_Profile1.Source = await CameraPhotoUserControl.LoadImage(_renderBitmapStorageFile);
                }
                else
                {
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
                }

                TextBlock_Require.Text = _post.Require ?? "";
                TextBlock_LookingFor.Text = _post.LookingFor;
                TextBlock_DressCpde.Text = "We're dressed like " + _post.DressCode;
                TextBlock_Description.Text = _post.OtherInfo ?? "";

                if(!string.IsNullOrWhiteSpace(_post.VenueId))
                {
                    TextBlock_ClubTitle.Text = _post.Place ?? "";
                    TextBlock_PlaceAddresse.Text = _post.PlaceAddresse ?? "";
                    TextBlock_ClubPopularity.Text = _post.VenuePopularity.ToString() + " users have been here";

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
                }
                else if(_post.OutType == "Anywhere")
                {
                    TextBlock_ClubTitle.Text = _post.Place;
                    TextBlock_PlaceAddresse.Text = "We could be anywhere if we find it interesting";
                } 
                else if(_post.OutType.StartsWith("Neighborhood"))
                {
                    TextBlock_ClubTitle.Text = "Neighborhood"; 
                    TextBlock_PlaceAddresse.Text = "Neighborhood of Metro station " +  _post.OutType.Substring(14);
                    Grid_NightClubPhoto.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    FlipView_NightClubPhoto.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    TextBlock_ClubTitle.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    TextBlock_PlaceAddresse.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    TextBlock_ClubPopularity.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
            catch (Exception)
            {

            }
        }

        public void StartPreviewMode()
        {
            if(_previewMode)
            {
                AppBarButton_Upload.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AppBarButton_Edit.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                TextBlock_PageTitle.Text = "Preview - " + App.MyPreviewPost.UserName ?? "";
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

        private void AppBarButton_Message_Click(object sender, RoutedEventArgs e)
        {
            if (_post != null)
            {
                var rootFrame = (Window.Current.Content as Frame);
                var chatEntry = new ChatEntry()
                {
                    Time = _post.ArrivalTime,
                    Message = "",
                    UserId = _post.UserId,
                    UserName = _post.UserName,
                    UserProfile = _post.ProfileUrl + "?Width=125"
                };
                //if (!rootFrame.Navigate(typeof(MessagePage), chatEntry))
                //{
                //    throw new Exception("Failed to create MainPage");
                //}
            }
        }

        private void AppBarButton_Edit_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.Navigate(typeof(PageTonightImGoingTo), true);
        }

        private async void AppBarButton_Upload_Click(object sender, RoutedEventArgs e)
        {
            Exception exp = null;
            try
            {
                string profileRemoteFileName = App.UserId + ".jpg";
                if (_renderBitmapStorageFile != null)
                    await (new AzureStorage()).UploadProfile(profileRemoteFileName, _renderBitmapStorageFile);
                using (HttpClient client = new HttpClient())
                {
                    var prms = new Dictionary<string, string>();
                    if (!string.IsNullOrWhiteSpace(profileRemoteFileName))
                        prms.Add("profileurl", "http://owlbat.azurewebsites.net/profile/" + profileRemoteFileName);
                    prms.Add("userid", App.MySelf.UserId);
                    string outType = "";
                    if (string.IsNullOrWhiteSpace(App.MyPreviewPost.VenueId) && App.MyPreviewPost.OutType == "Anywhere")
                        outType = "Anywhere";
                    else if (string.IsNullOrWhiteSpace(App.MyPreviewPost.VenueId) && App.MyPreviewPost.OutType.StartsWith("Neighborhood"))
                        outType = App.MyPreviewPost.OutType;
                    else
                        outType = "Venue";
                    prms.Add("outype", outType);
                    prms.Add("venueid", App.MyPreviewPost.VenueId ?? "");
                    prms.Add("lookingfor", App.MyPreviewPost.LookingFor ?? "");
                    prms.Add("arrivaltime", App.MyPreviewPost.ArrivalTime ?? "");
                    prms.Add("girlnumber", (App.MyPreviewPost.GirlsNumber ?? 0).ToString());
                    prms.Add("boynumber", (App.MyPreviewPost.GuysNumber ?? 0).ToString());
                    prms.Add("otherinfo", App.MyPreviewPost.OtherInfo ?? "");
                    prms.Add("codedress", App.MyPreviewPost.DressCode ?? "");

                    HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(prms);

                    client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                    HttpResponseMessage response = await client.PostAsync(new Uri("http://owlbat.azure-mobile.net/post/createpost"), formContent);
                    response.EnsureSuccessStatusCode();

                    await response.Content.ReadAsStringAsync();

                    if (response.Content != null && response.Content.ToString() == "")
                    {
                        var dialog = new MessageDialog(response.Content.ToString());
                        await dialog.ShowAsync();
                    }
                }

                App.MyPost = await JsonReceiver.GetPostByUserId(App.UserId);
                App.MyPreviewPost = App.MyPost.Clone();

                var rootFrame = (Window.Current.Content as Frame);
                rootFrame.Navigate(typeof(PivotPage));
            }
            catch(Exception expp)
            {
                exp = expp;
            }
            if (exp != null)
            {
                var dialog = new MessageDialog(exp.Message);
                await dialog.ShowAsync();
            }
        }
    }

    public class PostInfoNavigationParameter
    {
        public DataObjects.Post Post { get; set; }
        private bool _previewMode = false;
        public bool PreviewMode
        {
            get { return _previewMode; }
            set
            {
                _previewMode = value;
            }
        }

        public StorageFile RenderBitmapStorageFile { get; set; }
    }
}
