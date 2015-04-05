using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using Lumia.Imaging.Artistic;
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
        private readonly BitmapImage _capturedImage;
        private readonly StorageFile _storageFile;
        private IFilter _currentEffect;

        public ImageEffectsUserControl()
            : this(null, null)
        {

        }

        public ImageEffectsUserControl(BitmapImage capturedImage, StorageFile storageFile)
        {
            this.InitializeComponent();
            _capturedImage = capturedImage;
            _storageFile = storageFile;
            this.Loaded += ImageEffectsUserControl_Loaded;
        }

        void ImageEffectsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_capturedImage != null)
            {
                var bitmapTransform = new Windows.Graphics.Imaging.BitmapTransform();
                bitmapTransform.Flip = Windows.Graphics.Imaging.BitmapFlip.Vertical;
                bitmapTransform.Rotation = Windows.Graphics.Imaging.BitmapRotation.Clockwise90Degrees;
                Image_Captured.Source = _capturedImage;
            }
        }

        private void Image_Captured_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Image img = sender as Image;
            CompositeTransform ct = img.RenderTransform as CompositeTransform;
            if (ct == null) return;

            //Scale transform
            ct.ScaleX *= e.Delta.Scale;
            ct.ScaleY *= e.Delta.Scale;
            //if (ct.ScaleX < mincale) 
                //ct.ScaleX = mincale;
            //if (ct.ScaleY < mincale) 
                //ct.ScaleY = mincale;

            //Translate transform
            ct.TranslateX += e.Delta.Translation.X;
            ct.TranslateY += e.Delta.Translation.Y;

            //Confine boundary
            //BringIntoBounds();
        }

        public void BringIntoBounds()
        {
            //CompositeTransform ct = img.RenderTransform as CompositeTransform;
            //if (ct == null) return;

            ////out of screen, left edge
            //if (ct.TranslateX < 10 - img.ActualWidth * ct.ScaleX)
            //{
            //    ct.TranslateX = 10 - img.ActualWidth * ct.ScaleX;
            //}
            ////out of screen, right edge
            //if (ct.TranslateX > Container.ActualWidth - 10)
            //{
            //    ct.TranslateX = Container.ActualWidth - 10;
            //}
            //...do the same for Y.
        }

        public async void ChangeEffects(StorageFile file)
        {
            IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
            // Rewind the stream to start. 
            fileStream.Seek(0);

            // A cartoon effect is initialized with the selected image stream as source.  
            var imageSource = new RandomAccessStreamImageSource(fileStream);

            FilterEffect cartoonEffect = new FilterEffect(imageSource);
            cartoonEffect.Filters = new[] { _currentEffect };
            var cartoonImageBitmap = new WriteableBitmap((int)(Window.Current.Bounds.Width * 1),
                (int)(Window.Current.Bounds.Height * 1));
            // Render the image to a WriteableBitmap.
            var renderer = new WriteableBitmapRenderer(cartoonEffect, cartoonImageBitmap);
            cartoonImageBitmap = await renderer.RenderAsync();
            cartoonImageBitmap.Invalidate();

            // Set the rendered image as source for the cartoon image control.
            Image_Captured.Source = cartoonImageBitmap;
        }

        private async void AppBarButton_EffectsSolarize_Click(object sender, RoutedEventArgs e)
        {
            _currentEffect = new SolarizeFilter(0.9);
            StorageFile file = await StorageFile.GetFileFromPathAsync(_storageFile.Path);
            ChangeEffects(file);
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
            _currentEffect = filter;
            StorageFile file = await StorageFile.GetFileFromPathAsync(_storageFile.Path);
            ChangeEffects(file);
        }

        private async void AppBarButton_EffectsEnhence_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(_storageFile.Path);
            _currentEffect = new AutoEnhanceFilter();
            //StorageFile file = new StorageFile()
            ChangeEffects(file);
        }

        private async void AppBarButton_EffectsCartoon_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(_storageFile.Path);
            _currentEffect = new CartoonFilter();
            ChangeEffects(file);
        }
    }
}
