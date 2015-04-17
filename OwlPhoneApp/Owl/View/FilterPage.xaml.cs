using GalaSoft.MvvmLight.Messaging;
using OwlWindowsPhoneApp.ViewModel.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Telerik.UI.Xaml.Controls.Input.AutoCompleteBox;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OwlWindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FilterPage : Page
    {
        public FilterPage()
        {
            this.InitializeComponent();
            this.Loaded += FilterPage_Loaded;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void FilterPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitAutoTextComplete();
        }

        private void AppBarButton_Find_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<RefreshPostsMessage>(new RefreshPostsMessage());

            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.GoBack();
        }


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
