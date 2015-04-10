using OwlWindowsPhoneApp.View;
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

        void UserControl_FirstTimeEnter_GuideFinished(object sender, EventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            if (!rootFrame.Navigate(typeof(PivotPage)))
            {
                throw new Exception("Failed to create MainPage");
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
