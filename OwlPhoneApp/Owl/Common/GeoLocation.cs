using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Owl.Common
{
    public class GeoLocation
    {
        private Geolocator _geolocator = null;
        private CoreDispatcher _uiDispatcher;
        public event EventHandler<GeoLocationEventArg> GeoLocationUpdate;

        public GeoLocation(CoreDispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
            _geolocator = new Geolocator();
            _geolocator.ReportInterval = 50000;
            _geolocator.PositionChanged += Geolocator_PositionChanged;
        }

        async void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (GeoLocationUpdate != null)
                GeoLocationUpdate(this, new GeoLocationEventArg()
                {
                    Position = args.Position
                });
            //await _uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    Geoposition pos = args.Position;

            //    //var dialog = new MessageDialog(string.Format("GeoLocation: {0}, {1}", pos.Coordinate.Point.Position.Latitude, pos.Coordinate.Point.Position.Longitude));
            //    //dialog.Commands.Add(new UICommand("OK"));
            //    //await dialog.ShowAsync();
            //});
        }

        //public async void GeoLocationRequest()
        //{
        //    // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
        //    Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
        //    try
        //    {
        //        // Get cancellation token
        //        var cts = new CancellationTokenSource();
        //        CancellationToken token = cts.Token;

        //        // Carry out the operation
        //        Geoposition pos = await geolocator.GetGeopositionAsync().AsTask(token);

        //        var dialog = new MessageDialog(string.Format("GeoLocation: {0}, {1}, {3} - {4}, {5}",
        //            pos.CivicAddress.Country, pos.CivicAddress.City, pos.CivicAddress.PostalCode, pos.Coordinate.Point.Position.Latitude, pos.Coordinate.Point.Position.Longitude));
        //        dialog.Commands.Add(new UICommand("OK"));
        //        var returnCommand = await dialog.ShowAsync();
        //    }
        //    catch (System.UnauthorizedAccessException)
        //    {
        //    }
        //    catch (TaskCanceledException)
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //    }
        //}
    }

    public class GeoLocationEventArg : EventArgs
    {
        public Geoposition Position { get; set; }
    }
}
