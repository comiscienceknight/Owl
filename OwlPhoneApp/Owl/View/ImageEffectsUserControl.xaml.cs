using GalaSoft.MvvmLight.Messaging;
using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using Lumia.Imaging.Artistic;
using OwlWindowsPhoneApp.ViewModel.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Phone.UI.Input;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OwlWindowsPhoneApp.View
{
    public sealed partial class ImageEffectsUserControl : UserControl
    {
        public event EventHandler<ProfilePhotoRenderedEventArg> ProfilePhotoRendered;

        private readonly BitmapImage _capturedImage;
        private readonly string _storageFilePath;
        private StorageFile _storageFile;
        public int ProfileNumber = 0;

        public ImageEffectsUserControl()
            : this(null, null, null)
        {

        }

        public ImageEffectsUserControl(BitmapImage capturedImage, StorageFile storageFile, string storageFilePath)
        {
            this.InitializeComponent();
            _capturedImage = capturedImage;
            _storageFilePath = storageFilePath;
            _storageFile = storageFile;
            this.Loaded += ImageEffectsUserControl_Loaded;
        }

        async void ImageEffectsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_capturedImage != null)
            {
                var bitmapTransform = new Windows.Graphics.Imaging.BitmapTransform();
                bitmapTransform.Flip = Windows.Graphics.Imaging.BitmapFlip.Vertical;
                bitmapTransform.Rotation = Windows.Graphics.Imaging.BitmapRotation.Clockwise90Degrees;
                Image_Captured.Source = _capturedImage;

                Border_Image.Width = Window.Current.Bounds.Width - 20;
                Border_Image.Height = Border_Image.Width * 0.9;
            }
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
                        Image_Captured.Source = cartoonImageBitmap;
                    }
                }
            }
        }

        private async void AppBarButton_EffectsSolarize_Click(object sender, RoutedEventArgs e)
        {
            var filter = new SolarizeFilter(0.9);
            StorageFile file = await StorageFile.GetFileFromPathAsync(_storageFilePath);
            ChangeEffects(file, filter);
        }

        private async void AppBarButton_EffectsBlur_Click(object sender, RoutedEventArgs e)
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

            StorageFile file = await StorageFile.GetFileFromPathAsync(_storageFilePath);
            ChangeEffects(file, filter);
        }

        private async void AppBarButton_EffectsEnhence_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(_storageFile.Path);
            var filter = new AutoEnhanceFilter();
            //StorageFile file = new StorageFile()
            ChangeEffects(file, filter);
        }

        private async void AppBarButton_EffectsCartoon_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(_storageFilePath);
            var filter = new CartoonFilter();
            ChangeEffects(file, filter);
        }

        private async void AppBarButton_Save_Click(object sender, RoutedEventArgs e)
        {
            var bmp = await CreateBitmapFromElement(this.Grid_Image);
            if (ProfilePhotoRendered != null)
                ProfilePhotoRendered(this, new ProfilePhotoRenderedEventArg()
                {
                    ProfilePhotoNumber = this.ProfileNumber,
                    TakedPhotoImage = bmp
                });
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
    }

    public class ProfilePhotoRenderedEventArg : EventArgs
    {
        public int ProfilePhotoNumber { get; set; }
        public RenderTargetBitmap TakedPhotoImage { get; set; }
    }
}
