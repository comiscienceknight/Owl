using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Owl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        HubConnection _hubConnection;
        Common.GeoLocation _geoLocation;

        public ObservableCollection<string> GeoPositions = new ObservableCollection<string>();

        public BlankPage()
        {
            this.InitializeComponent();
            this.Loaded += BlankPage_Loaded;
        }

        void BlankPage_Loaded(object sender, RoutedEventArgs e)
        {
            _geoLocation = new Common.GeoLocation(this.Dispatcher);
            _geoLocation.GeoLocationUpdate += GeoLocation_GeoLocationUpdate;
            InitGps();
            ListView_GeoPositions.ItemsSource = GeoPositions;
        }

        async void GeoLocation_GeoLocationUpdate(object sender, Common.GeoLocationEventArg e)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                var prms = new Dictionary<string, string>();
                prms.Add("UserId", "Bo HU");
                prms.Add("Altitude", e.Position.Coordinate.Latitude.ToString());
                prms.Add("Longitude", e.Position.Coordinate.Longitude.ToString());

                try
                {
                    HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(prms);
                    HttpResponseMessage response = await client.PostAsync(new Uri("http://owlbat.azure-mobile.net/api/updatepos"), formContent);
                    response.EnsureSuccessStatusCode();

                    await response.Content.ReadAsStringAsync();

                    if (response.Content != null && response.Content.ToString() != "")
                    {
                        var dialog = new MessageDialog(response.Content.ToString());
                        await dialog.ShowAsync();
                    }
                }
                catch(Exception exp)
                {

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

        
        private async void InitGps()
        {
            if (_hubConnection == null)
            {
                _hubConnection = new HubConnection(App.OwlbatClient.ApplicationUri.AbsoluteUri);
                if (App.OwlbatClient != null)
                {
                    _hubConnection.Headers["x-zumo-auth"] = App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken;
                }
                else
                {
                    _hubConnection.Headers["x-zumo-application"] = App.OwlbatClient.ApplicationKey;
                }

                IHubProxy proxy = _hubConnection.CreateHubProxy("GpsHub");
                await _hubConnection.Start();

                proxy.On<string>("Send", async msg =>
                {
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        GeoPositions.Add(msg);
                    });
                });
            }
        }
    }
}
