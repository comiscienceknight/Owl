using Owl.Models;
using OwlWindowsPhoneApp.DataObjects;
using OwlWindowsPhoneApp.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Input.AutoCompleteBox;
using Windows.Data.Json;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OwlWindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditMyProfilePage : Page, IFileOpenPickerContinuable
    {
        private Post _post;
        public bool IsTakingPhoto = false;
        private CameraPhotoUserControl _cameraUc;
        private ImageEffectsUserControl _imageEffectsUc;
        private int _selectProfile = -1;

        public EditMyProfilePage()
        {
            this.InitializeComponent();
            this.Loaded += EditMyProfilePage_Loaded;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _post = e.Parameter as Post;
        }

        void EditMyProfilePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(_post.ProfileUrl))
            {
                Uri myUri = new Uri(_post.ProfileUrl, UriKind.Absolute);
                BitmapImage bmi = new BitmapImage();
                bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmi.UriSource = myUri;
                Image_Profile1.Source = bmi;
            }
            if (!string.IsNullOrWhiteSpace(_post.ProfileUrl2))
            {
                Uri myUri = new Uri(_post.ProfileUrl2, UriKind.Absolute);
                BitmapImage bmi = new BitmapImage();
                bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmi.UriSource = myUri;
                Image_Profile2.Source = bmi;
            }
            if (!string.IsNullOrWhiteSpace(_post.ProfileUrl3))
            {
                Uri myUri = new Uri(_post.ProfileUrl3, UriKind.Absolute);
                BitmapImage bmi = new BitmapImage();
                bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmi.UriSource = myUri;
                Image_Profile3.Source = bmi;
            }
            
            RadAutoCompleteBox_Search.Text = _post.Place;
            NumericUpDown_WithBoys.Value = _post.GuysNumber ?? 0;
            NumericUpDown_WithGirls.Value = _post.GirlsNumber ?? 0;
            TextBox_NickName.Text = _post.UserName;
            

            InitAutoTextComplete();
        }

        private async void AppBarButton_Upload_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string profileUrl1 = null, profileUrl2 = null, profileUrl3 = null;
            if(Image_Profile1.Source != null && Image_Profile1.Source is RenderTargetBitmap)
            {
                profileUrl1 = "owlUploadingPic" + ".jpeg";
                await SaveProfile(Image_Profile1.Source as RenderTargetBitmap, profileUrl1);
                string profileRemoteFileName = App.UserId + ".jpg";
                StorageFile savedFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(profileUrl1);
                await (new AzureStorage()).UploadProfile(profileRemoteFileName, savedFile);
                profileUrl1 = profileRemoteFileName;
            }
            if (Image_Profile2.Source != null && Image_Profile2.Source is RenderTargetBitmap)
            {
                profileUrl2 = "owlUploadingPic2" + ".jpeg";
                await SaveProfile(Image_Profile2.Source as RenderTargetBitmap, profileUrl2);
                string profileRemoteFileName2 = App.UserId + "2.jpg";
                StorageFile savedFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(profileUrl2);
                await (new AzureStorage()).UploadProfile(profileRemoteFileName2, savedFile);
                profileUrl2 = profileRemoteFileName2;
            }
            if (Image_Profile3.Source != null && Image_Profile3.Source is RenderTargetBitmap)
            {
                profileUrl3 = "owlUploadingPic3" + ".jpeg";
                await SaveProfile(Image_Profile3.Source as RenderTargetBitmap, profileUrl3);
                string profileRemoteFileName3 = App.UserId + "3.jpg";
                StorageFile savedFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(profileUrl3);
                await (new AzureStorage()).UploadProfile(profileRemoteFileName3, savedFile);
                profileUrl3 = profileRemoteFileName3;
            }

            var venueInfo = RadAutoCompleteBox_Search.SelectedItem as SearchAvenues;
            if (venueInfo == null || string.IsNullOrWhiteSpace(venueInfo.VenueId))
            {
                if(_post.Place != RadAutoCompleteBox_Search.Text)
                {
                    var dialog = new MessageDialog("Venue should be recognized");
                    await dialog.ShowAsync();
                    return;
                }
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);

                var prms = new Dictionary<string, string>();
                if(!string.IsNullOrWhiteSpace(profileUrl1))
                    prms.Add("profileurl", "http://owlbat.azurewebsites.net/profile/" + profileUrl1);
                if (!string.IsNullOrWhiteSpace(profileUrl2))
                    prms.Add("profileurl2", "http://owlbat.azurewebsites.net/profile/" + profileUrl2);
                if (!string.IsNullOrWhiteSpace(profileUrl3))
                    prms.Add("profileurl3", "http://owlbat.azurewebsites.net/profile/" + profileUrl3);
                prms.Add("agerange", TextBlock_AgeRange.Text ?? "");
                prms.Add("description", (TextBlock_Description.Text ?? "").Replace("'", "''"));
                prms.Add("girlsnumber", ((int)NumericUpDown_WithGirls.Value).ToString());
                prms.Add("guysnumber", ((int)NumericUpDown_WithGirls.Value).ToString());
                prms.Add("username", TextBox_NickName.Text ?? " ");
                prms.Add("venueid", venueInfo == null ? _post.VenueId : venueInfo.VenueId);
                prms.Add("venuename", venueInfo == null ? _post.Place : venueInfo.Venue);
                prms.Add("codedress", TextBlock_DressCode.Text ?? "");
                prms.Add("userid", App.UserId);

                HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(prms);
                HttpResponseMessage response = await client.PostAsync(new Uri("http://owlbat.azure-mobile.net/post/updatepost"), formContent);
                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync();

                if (response.Content != null && response.Content.ToString() == "")
                {
                    var dialog = new MessageDialog(response.Content.ToString());
                    await dialog.ShowAsync();
                }
            }

            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.GoBack();
        }

        private async Task SaveProfile(RenderTargetBitmap bmp, string imageName)
        {
            var pixels = await bmp.GetPixelsAsync();
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(imageName, CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await
                    BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                byte[] bytes = pixels.ToArray();
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Ignore,
                                     (uint)bmp.PixelWidth, (uint)bmp.PixelHeight,
                                     96, 96, bytes);
                await encoder.FlushAsync();
            }
        }

        #region photo profile
        private void Image_Profile1_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Grid_ManipForProfile.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Image_SelectedImage.Source = Image_Profile1.Source;
            Image_SelectedImage.Width = Window.Current.Bounds.Width - 40;
            Image_SelectedImage.Height = Image_SelectedImage.Width * 0.9;
            Grid_ManipForProfile.Height = Image_SelectedImage.Height + 150;
            Grid_ManipForProfile.Width = Window.Current.Bounds.Width - 34;
            _selectProfile = 0;
        }

        private void AppBarButton_InsertProfile2_Click(object sender, RoutedEventArgs e)
        {
            Grid_ManipForProfile.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if(Image_Profile2.Source != null)
                Image_SelectedImage.Source = Image_Profile2.Source;
            else
                Image_SelectedImage.Source = null;
            Image_SelectedImage.Width = Window.Current.Bounds.Width - 40;
            Image_SelectedImage.Height = Image_SelectedImage.Width * 0.9;
            Grid_ManipForProfile.Height = Image_SelectedImage.Height + 150;
            Grid_ManipForProfile.Width = Window.Current.Bounds.Width - 34;
            _selectProfile = 1;
        }

        private void AppBarButton_InsertProfile3_Click(object sender, RoutedEventArgs e)
        {
            Grid_ManipForProfile.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (Image_Profile3.Source != null)
                Image_SelectedImage.Source = Image_Profile3.Source;
            else
                Image_SelectedImage.Source = null;
            Image_SelectedImage.Width = Window.Current.Bounds.Width - 40;
            Image_SelectedImage.Height = Image_SelectedImage.Width * 0.9;
            Grid_ManipForProfile.Height = Image_SelectedImage.Height + 150;
            Grid_ManipForProfile.Width = Window.Current.Bounds.Width - 34;
            _selectProfile = 2;
        }

        private async void AppBarButton_RemovePhoto_Click(object sender, RoutedEventArgs e)
        {
            if(Image_SelectedImage.Source == Image_Profile1.Source)
            {
                var dialog = new MessageDialog("Main profile cannot be removed");
                dialog.Commands.Add(new UICommand("Ok"));
                await dialog.ShowAsync();
            }
            else
            {
                if (Image_SelectedImage.Source == Image_Profile2.Source)
                {
                    _post.Profile2 = null;
                    _post.ProfileUrl2 = null;
                }
                else if (Image_SelectedImage.Source == Image_Profile3.Source)
                {
                    _post.Profile3 = null;
                    _post.ProfileUrl3 = null;
                }
                Image_SelectedImage.Source = null;
                Grid_ManipForProfile.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void AppBarButton_CloseProfileManip_Click(object sender, RoutedEventArgs e)
        {
            Grid_ManipForProfile.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void AppBarButton_ChangeProfile_Click(object sender, RoutedEventArgs e)
        {
            Grid_SubWindow.Children.Clear();
            _cameraUc = new CameraPhotoUserControl();
            _cameraUc.ProfileNumber = _selectProfile;
            _cameraUc.TakePhotoEvent += CameraPhotoUserControl_TakePhotoEvent;
            _cameraUc.ChoosePhotoFromStorageEvent += CameraPhotoUserControl_ChoosePhotoFromStorageEvent;
            Grid_SubWindow.Children.Add(_cameraUc);
            Grid_SubWindow.Visibility = Windows.UI.Xaml.Visibility.Visible;
            IsTakingPhoto = true;
            AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Grid_ManipForProfile.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        void CameraPhotoUserControl_ChoosePhotoFromStorageEvent(object sender, ChoosePhotoFromStorageClickEventArg e)
        {
            if (_cameraUc != null)
            {
                _cameraUc.TakePhotoEvent -= CameraPhotoUserControl_TakePhotoEvent;
                _cameraUc.ChoosePhotoFromStorageEvent -= CameraPhotoUserControl_ChoosePhotoFromStorageEvent;
                Grid_SubWindow.Children.Clear();
            }
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.PickSingleFileAndContinue();
        }

        void CameraPhotoUserControl_TakePhotoEvent(object sender, TakePhotoClickEventArg e)
        {
            if (_cameraUc != null)
            {
                _cameraUc.TakePhotoEvent -= CameraPhotoUserControl_TakePhotoEvent;
                _cameraUc.ChoosePhotoFromStorageEvent -= CameraPhotoUserControl_ChoosePhotoFromStorageEvent;
                Grid_SubWindow.Children.Clear();

                _imageEffectsUc = new ImageEffectsUserControl(e.TakedPhotoImage, e.TakedPhotoFile, e.TakedPhotoFile.Path);
                _imageEffectsUc.ProfileNumber = e.ProfilePhotoNumber;
                _imageEffectsUc.ProfilePhotoRendered += ImageEffectsUc_ProfilePhotoRendered;
                Grid_SubWindow.Children.Add(_imageEffectsUc);
                Grid_SubWindow.Visibility = Windows.UI.Xaml.Visibility.Visible;

                IsTakingPhoto = true;
                AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        void ImageEffectsUc_ProfilePhotoRendered(object sender, ProfilePhotoRenderedEventArg e)
        {
            _imageEffectsUc.ProfilePhotoRendered -= ImageEffectsUc_ProfilePhotoRendered;
            Grid_SubWindow.Children.Clear();
            _imageEffectsUc = null;
            if(e.ProfilePhotoNumber == 0)
            {
                Image_Profile1.Source = e.TakedPhotoImage;
            }
            else if (e.ProfilePhotoNumber == 1)
            {
                Image_Profile2.Source = e.TakedPhotoImage;
            }
            else if (e.ProfilePhotoNumber == 2)
            {
                Image_Profile3.Source = e.TakedPhotoImage;
            }
            AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        public void CloseCameraByBackButton()
        {
            if(_cameraUc != null)
            {
                _cameraUc.TakePhotoEvent -= CameraPhotoUserControl_TakePhotoEvent;
                _cameraUc.ChoosePhotoFromStorageEvent -= CameraPhotoUserControl_ChoosePhotoFromStorageEvent;
            }
            if (_imageEffectsUc != null)
            {
                _imageEffectsUc.ProfilePhotoRendered -= ImageEffectsUc_ProfilePhotoRendered;
            }
            Grid_SubWindow.Children.Clear();
            Grid_SubWindow.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            IsTakingPhoto = false;
            _cameraUc = null;
            _imageEffectsUc = null;
            _selectProfile = -1;
            AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        public async void ContinueFileOpenPicker(Windows.ApplicationModel.Activation.FileOpenPickerContinuationEventArgs args)
        {
            if (args.Files.Count > 0)
            {
                var file = args.Files.FirstOrDefault();
                if (file == null)
                    return;

                StorageFile sf = args.Files.First();
                BitmapImage bmpImage = await CameraPhotoUserControl.LoadImage(sf);
                _imageEffectsUc = new ImageEffectsUserControl(bmpImage, sf, sf.Path);
                _imageEffectsUc.ProfileNumber = _selectProfile;
                _imageEffectsUc.ProfilePhotoRendered += ImageEffectsUc_ProfilePhotoRendered;
                Grid_SubWindow.Children.Clear();
                Grid_SubWindow.Children.Add(_imageEffectsUc);
            }
            else
            {
                IsTakingPhoto = false;
                _cameraUc = null;
                _imageEffectsUc = null;
                _selectProfile = -1;
            }
        }
        #endregion


        #region avenues search
        private WebServiceTextSearchProvider _provider;
        private void InitAutoTextComplete()
        {
            _provider = new WebServiceTextSearchProvider();
            _provider.InputChanged += WebServiceProvider_InputChanged;
            RadAutoCompleteBox_Search.InitializeSuggestionsProvider(_provider);
        }

        private async void WebServiceProvider_InputChanged(object sender, EventArgs e)
        {
            string inputString = _provider.InputString;
            if (!string.IsNullOrEmpty(inputString))
            {
                this.ProgressBar_Search.Visibility = Visibility.Visible;

                List<SearchAvenues> venues = await SearchAvenues(inputString);

                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    _provider.LoadItems(venues.OrderBy(p => p.Venue));
                });

                this.ProgressBar_Search.Visibility = Visibility.Collapsed;
            }
            else
            {
                _provider.Reset();
            }
        }

        private async Task<List<SearchAvenues>> SearchAvenues(string inputString)
        {
            List<SearchAvenues> venueResult = new List<SearchAvenues>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                var venues = await httpClient.GetStringAsync(
                    new Uri("http://owlbat.azure-mobile.net/get/getvenuesbi/" + inputString));
                JsonValue jsonValue = JsonValue.Parse(venues);
                AnalysePostJsonValueArray(jsonValue, venueResult);
            }
            return venueResult;
        }

        private void AnalysePostJsonValueArray(JsonValue venuesJasonValue, List<SearchAvenues> venueResult)
        {
            if (venuesJasonValue.ValueType == JsonValueType.Array)
            {
                foreach (var jv in venuesJasonValue.GetArray().ToList())
                {
                    venueResult.Add(AnalyseVenueJsonValue(jv));
                }
            }
        }

        private SearchAvenues AnalyseVenueJsonValue(IJsonValue postJasonValue)
        {
            JsonObject jo = postJasonValue.GetObject();
            var venue = new SearchAvenues();

            venue.VenueId = jo.GetNamedString("id");

            if (jo.ContainsKey("placeName"))
                venue.Venue = jo.GetNamedString("placeName");

            if (jo.ContainsKey("placeAddresse"))
                venue.Adresse = jo.GetNamedString("placeAddresse");

            return venue;
        }
        #endregion
    }
}
