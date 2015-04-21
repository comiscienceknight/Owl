using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using Lumia.Imaging.Artistic;
using Owl.Models;
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

namespace Owl.View.FirstVisit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageProfile : Page, IFileOpenPickerContinuable
    {
        private StorageFile _selectedStorageFile = null;
        private BitmapImage _capturedImage;

        public PageProfile()
        {
            this.InitializeComponent();
            this.Loaded += PageProfile_Loaded;
        }

        void PageProfile_Loaded(object sender, RoutedEventArgs e)
        {
            Border_Image.Width = Window.Current.Bounds.Width - 20;
            Border_Image.Height = Border_Image.Width * 0.9;

            if(App.MyPost != null && !string.IsNullOrWhiteSpace(App.MyPost.ProfileUrl) &&
               (App.MyPost.ProfileUrl.ToUpper().EndsWith(".JPEG") || App.MyPost.ProfileUrl.ToUpper().EndsWith(".JPEG") || 
                App.MyPost.ProfileUrl.ToUpper().EndsWith(".PNG")))
            {
                if(App.MyPost.Profile != null)
                {
                    Image_Profile.Source = App.MyPost.Profile;
                    AppBarButton_Upload.IsEnabled = true;
                }
                else
                {
                    try
                    {
                        Uri myUri = new Uri(App.MyPost.ProfileUrl, UriKind.Absolute);
                        BitmapImage bmi = new BitmapImage();
                        bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                        bmi.UriSource = myUri;
                        Image_Profile.Source = bmi;
                        AppBarButton_Upload.IsEnabled = true;
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void Image_CarttonEffect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(_selectedStorageFile != null)
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(_selectedStorageFile.Path);
                var filter = new CartoonFilter();
                ChangeEffects(file, filter);
            }
        }

        private async void Image_EnhanceEffect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_selectedStorageFile != null)
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(_selectedStorageFile.Path);
                var filter = new AutoEnhanceFilter();
                //StorageFile file = new StorageFile()
                ChangeEffects(file, filter);
            }
        }

        private async void Image_BlurEffect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_selectedStorageFile != null)
            {
                var filter = new HueSaturationLightnessFilter();

                filter.HueCurve = new Curve();
                filter.HueCurve.SetPoint(0, 0);
                filter.HueCurve.SetPoint(10, 20);
                filter.HueCurve.SetPoint(200, 500);

                filter.SaturationCurve = new Curve();
                filter.SaturationCurve.SetPoint(0, 0);
                filter.SaturationCurve.SetPoint(10, 20);
                filter.SaturationCurve.SetPoint(200, 240);

                filter.LightnessCurve = new Curve();
                filter.LightnessCurve.SetPoint(0, 0);
                filter.LightnessCurve.SetPoint(10, 20);
                filter.LightnessCurve.SetPoint(200, 240);

                var file = await StorageFile.GetFileFromPathAsync(_selectedStorageFile.Path);
                ChangeEffects(file, filter);
            }
        }

        private void Image_Profile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.PickSingleFileAndContinue();
        }

        public async void ContinueFileOpenPicker(Windows.ApplicationModel.Activation.FileOpenPickerContinuationEventArgs args)
        {
            if (args.Files.Count > 0)
            {
                var file = args.Files.FirstOrDefault();
                if (file == null)
                    return;

                _selectedStorageFile = args.Files.First();
                _capturedImage = await CameraPhotoUserControl.LoadImage(_selectedStorageFile);

                Image_Profile.Source = _capturedImage;
                AppBarButton_Upload.IsEnabled = true;
            }
        }

        private void AppBarButton_InsertProfile_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.PickSingleFileAndContinue();
        }

        public async void ChangeEffects(StorageFile file, IFilter filter)
        {
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                // Rewind the stream to start. 
                fileStream.Seek(0);

                using (var imageSource = new RandomAccessStreamImageSource(fileStream))
                {
                    using (FilterEffect effect = new FilterEffect(imageSource))
                    {
                        effect.Filters = new[] { filter };

                        var cartoonImageBitmap = new WriteableBitmap((int)(_capturedImage.PixelWidth),
                            (int)(_capturedImage.PixelHeight));

                        // Render the image to a WriteableBitmap.
                        var renderer = new WriteableBitmapRenderer(effect, cartoonImageBitmap);
                        cartoonImageBitmap = await renderer.RenderAsync();
                        cartoonImageBitmap.Invalidate();

                        // Set the rendered image as source for the cartoon image control.
                        Image_Profile.Source = cartoonImageBitmap;
                        AppBarButton_Upload.IsEnabled = true;
                    }
                }
            }
        }

        private async void AppBarButton_Upload_Click(object sender, RoutedEventArgs e)
        {
            var renderBitmap = await CreateBitmapFromElement(Grid_Image);
            var profileUrl = "owlUploadingPic" + ".jpeg";
            string profileRemoteFileName = App.UserId + ".jpg";
            if (renderBitmap != null)
            {
                await SaveProfile(renderBitmap, profileUrl);
                StorageFile savedFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(profileUrl);
                await(new AzureStorage()).UploadProfile(profileRemoteFileName, savedFile);
                profileUrl = profileRemoteFileName;
            }
            using (HttpClient client = new HttpClient())
            {
                var prms = new Dictionary<string, string>();
                if (!string.IsNullOrWhiteSpace(profileUrl))
                    prms.Add("profileurl", "http://owlbat.azurewebsites.net/profile/" + profileUrl);
                prms.Add("userid", App.MyPost.UserId);
                string outType = "";
                if (string.IsNullOrWhiteSpace(App.MyPost.VenueId) && App.MyPost.Place == "Anywhere")
                    outType = "Anywhere";
                else
                    outType = "Venue";
                prms.Add("outype", outType);
                prms.Add("venueid", App.MyPost.VenueId ?? "");
                prms.Add("lookingfor", App.MyPost.LookingFor ?? "");
                prms.Add("arrivaltime", App.MyPost.Time ?? "");
                prms.Add("girlnumber", (App.MyPost.GirlsNumber ?? 0).ToString());
                prms.Add("boynumber", (App.MyPost.GuysNumber ?? 0).ToString());
                prms.Add("otherinfo", App.MyPost.OtherInfo ?? "");
                prms.Add("codedress", App.MyPost.DressCode ?? "");

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

            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.Navigate(typeof(Owl.PivotPage));
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

        private async Task<RenderTargetBitmap> CreateBitmapFromElement(FrameworkElement uielement)
        {
            try
            {
                var renderTargetBitmap = new RenderTargetBitmap();
                await renderTargetBitmap.RenderAsync(uielement);

                return renderTargetBitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return null;
        }

        private void AppBarButton_Back_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.GoBack();
        }

        private void AppBarButton_Leave_Click(object sender, RoutedEventArgs e)
        {
            App.QuitFromEditPost();
        }

        private void ScrollViewer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.PickSingleFileAndContinue();
        }
    }
}
