using Owl.Models;
using OwlWindowsPhoneApp.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OwlWindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstEnterIn : Page, IFileOpenPickerContinuable
    {
        public FirstEnterIn()
        {
            this.InitializeComponent();
            this.Loaded += FirstEnterIn_Loaded;
        }

        void FirstEnterIn_Loaded(object sender, RoutedEventArgs e)
        {
            UserControl_FirstTimeEnter.GuideFinished += UserControl_FirstTimeEnter_GuideFinished;
            UserControl_FirstTimeEnter.TakePhotoClick += UserControl_FirstTimeEnter_TakePhotoClick;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void UserControl_FirstTimeEnter_TakePhotoClick(object sender, EventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.PickSingleFileAndContinue();
        }

        async void UserControl_FirstTimeEnter_GuideFinished(object sender, GuideFinishedEventArgs e)
        {
            Grid_Uploading.Visibility = Windows.UI.Xaml.Visibility.Visible;

            if(e.UserId != "$Existed$")
            {
                await UploadCreatedInfo(e);
            }

            var rootFrame = (Window.Current.Content as Frame);
            if (!rootFrame.Navigate(typeof(PivotPage)))
            {
                throw new Exception("Failed to create MainPage");
            }
        }

        private async Task UploadCreatedInfo(GuideFinishedEventArgs arg)
        {
            var pixels = await arg.ProfileBmp.GetPixelsAsync();
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync("owlUploadingPic.jpeg", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await
                    BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                byte[] bytes = pixels.ToArray();
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Ignore,
                                     (uint)arg.ProfileBmp.PixelWidth, (uint)arg.ProfileBmp.PixelHeight,
                                     96, 96, bytes);

                await encoder.FlushAsync();
            }

            string profileRemoteFileName = App.OwlbatClient.CurrentUser.UserId.Replace(":", "") + ".jpg";
            StorageFile savedFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("owlUploadingPic.jpeg");
            await(new AzureStorage()).UploadProfile(profileRemoteFileName, savedFile);
            
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);

                var prms = new Dictionary<string, string>();
                prms.Add("profileurl", "http://owlbat.azurewebsites.net/profile/" + profileRemoteFileName);
                prms.Add("agerange", arg.AgeRange);
                prms.Add("description", arg.Description);
                prms.Add("girlsnumber", arg.GirlsNumber.ToString());
                prms.Add("guysnumber", arg.GuysNumber.ToString());
                prms.Add("sexe", arg.Sexe);
                prms.Add("username", arg.UserName);
                prms.Add("venueid", arg.VenueId);
                prms.Add("venuename", arg.VenueName);
                prms.Add("userid", App.OwlbatClient.CurrentUser.UserId);
                //prms.Add("id", Guid.NewGuid().ToString());

                HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(prms);
                HttpResponseMessage response = await client.PostAsync(new Uri("http://owlbat.azure-mobile.net/post/addpost"), formContent);
                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync();
                var dialog = new MessageDialog(response.Content.ToString());
                await dialog.ShowAsync();
            }
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
                ImageEffectsUserControl imageEffectUc = new ImageEffectsUserControl(bmpImage, sf, sf.Path);
                imageEffectUc.ProfilePhotoRendered += ImageEffectUc_ProfilePhotoRendered;
                Grid_ImageEffects.Children.Clear();
                Grid_ImageEffects.Children.Add(imageEffectUc);
            }
        }

        void ImageEffectUc_ProfilePhotoRendered(object sender, ProfilePhotoRenderedEventArg e)
        {
            ((ImageEffectsUserControl)sender).ProfilePhotoRendered -= ImageEffectUc_ProfilePhotoRendered;
            Grid_ImageEffects.Children.Clear();
            UserControl_FirstTimeEnter.ChangeImageProfile(e.TakedPhotoImage);
        }

    }
}
