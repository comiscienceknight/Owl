using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OwlWindowsPhoneApp
{
    public sealed partial class CameraPhotoUserControl : UserControl
    {
        private MediaCapture _captureManager;
        private DeviceInformationCollection _webcamList;
        private DeviceInformation _frontWebcam;
        private DeviceInformation _backWebcam;
        private int _manipStncObjBool = 0;

        public CameraPhotoUserControl()
        {
            this.InitializeComponent();
            this.Loaded += CameraPhotoUserControl_Loaded;
            this.Unloaded += CameraPhotoUserControl_Unloaded;
        }

        async void CameraPhotoUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            await _captureManager.StopPreviewAsync();
            CaptureElement_Photo.Source.Dispose();
        }

        async void CameraPhotoUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CaptureElement_Photo.Width = Window.Current.Bounds.Width;
            CaptureElement_Photo.Height = Window.Current.Bounds.Height;
            await GetAllVideoDevice();
            StartPreview((_backWebcam != null ? _backWebcam.Id : (_frontWebcam != null ? _frontWebcam.Id : null)));
        }

        private async Task GetAllVideoDevice()
        {
            _webcamList = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            // Then I do a query to find the front webcam
            if (_webcamList.Any(webcam => webcam.EnclosureLocation != null
                                             && webcam.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front))
                _frontWebcam = (from webcam in _webcamList
                                where webcam.EnclosureLocation != null
                                && webcam.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front
                                select webcam).FirstOrDefault();
            if (_webcamList.Any(webcam => webcam.EnclosureLocation != null
                                             && webcam.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back))
                // Same for the back webcam
                _backWebcam = (from webcam in _webcamList
                               where webcam.EnclosureLocation != null
                               && webcam.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back
                               select webcam).FirstOrDefault();
            return;
        }

        public async void StartPreview(string videoDeviceId, bool if270Degree = true)
        {
            _captureManager = new MediaCapture();
            await _captureManager.InitializeAsync(new MediaCaptureInitializationSettings
            {
                StreamingCaptureMode = StreamingCaptureMode.Video,
                PhotoCaptureSource = PhotoCaptureSource.VideoPreview,
                AudioDeviceId = string.Empty,
                VideoDeviceId = videoDeviceId
            });
            _captureManager.SetPreviewRotation(if270Degree ? VideoRotation.Clockwise90Degrees : VideoRotation.Clockwise270Degrees);
            _captureManager.SetRecordRotation(if270Degree ? VideoRotation.Clockwise90Degrees : VideoRotation.Clockwise270Degrees);
            CaptureElement_Photo.Source = _captureManager;
            await _captureManager.StartPreviewAsync();
        }

        private async void AppBarButton_RotateCamera_Click(object sender, RoutedEventArgs e)
        {
            if (Interlocked.CompareExchange(ref _manipStncObjBool, 1, 0) == 0)
            {
                try
                {
                    await _captureManager.StopPreviewAsync();
                    string backId = (_backWebcam == null ? "" : _backWebcam.Id);
                    string frontId = (_frontWebcam == null ? "" : _frontWebcam.Id);
                    if (_captureManager.MediaCaptureSettings.VideoDeviceId.ToUpper() == backId.ToUpper())
                    {
                        StartPreview(frontId, false);
                        AppBarButton_RotateCamera.Label = "Back Camera";
                    }
                    else if (_captureManager.MediaCaptureSettings.VideoDeviceId.ToUpper() == frontId.ToUpper())
                    {
                        StartPreview(backId);
                        AppBarButton_RotateCamera.Label = "Front Camera";
                    }
                }
                finally
                {
                    Interlocked.CompareExchange(ref _manipStncObjBool, 0, 1);
                }
            }
        }

        private async void AppBarButton_Focus_Click(object sender, RoutedEventArgs e)
        {
            string backId = (_backWebcam == null ? "" : _backWebcam.Id);
            if (_captureManager.MediaCaptureSettings.VideoDeviceId.ToUpper() == backId.ToUpper())
            {
                //await _captureManager.VideoDeviceController.FocusControl.LockAsync();
                await _captureManager.VideoDeviceController.FocusControl.FocusAsync();
                //await _captureManager.VideoDeviceController.FocusControl.UnlockAsync();
            }
        }

        private async void AppBarButton_Focus_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

        }
    }
}
