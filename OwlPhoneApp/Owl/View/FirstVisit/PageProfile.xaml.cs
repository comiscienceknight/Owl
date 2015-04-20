using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Owl.View.FirstVisit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageProfile : Page, IFileOpenPickerContinuable
    {
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
                }
                else
                {
                    Uri myUri = new Uri(App.MyPost.ProfileUrl, UriKind.Absolute);
                    BitmapImage bmi = new BitmapImage();
                    bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmi.UriSource = myUri;
                    Image_Profile.Source = bmi;
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

        private void Image_CarttonEffect_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void Image_EnhanceEffect_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void Image_BlurEffect_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void Button_Goback_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.GoBack();
        }

        private void Button_Preview_Click(object sender, RoutedEventArgs e)
        {

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

                StorageFile sf = args.Files.First();
                BitmapImage bmpImage = await CameraPhotoUserControl.LoadImage(sf);

                Image_Profile.Source = bmpImage;
            }
        }
    }
}
