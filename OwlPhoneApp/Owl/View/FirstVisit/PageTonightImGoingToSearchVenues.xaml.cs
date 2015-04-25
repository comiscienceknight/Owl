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

namespace Owl.View.FirstVisit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageTonightImGoingToSearchVenues : Page
    {
        public PageTonightImGoingToSearchVenues()
        {
            this.InitializeComponent();
            this.Loaded += PageTonightImGoingToSearchVenues_Loaded;
        }

        void PageTonightImGoingToSearchVenues_Loaded(object sender, RoutedEventArgs e)
        {
            InitAutoTextComplete();

            if (App.MyPost != null)
            {
                if (App.MyPost.VenueId != null)
                {
                    RadAutoCompleteBox_Search.Text = App.MyPost.Place;
                    AppBarButton_Forward.IsEnabled = true;
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

        private void RadAutoCompleteBox_Search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchAvenues sa = RadAutoCompleteBox_Search.SelectedItem as SearchAvenues;
            if (sa != null && !string.IsNullOrWhiteSpace(sa.VenueId))
            {
                App.MyPost.VenueId = sa.VenueId;
                App.MyPost.Place = sa.Venue;
                App.MyPost.PlaceAddresse = sa.Adresse;

                AppBarButton_Forward.IsEnabled = true;
            }
        }


        #region avenues search
        private WebServiceTextSearchProvider _provider;

        private void InitAutoTextComplete()
        {
            _provider = new WebServiceTextSearchProvider();
            _provider.InputChanged += WebServiceProvider_InputChanged;
            RadAutoCompleteBox_Search.InitializeSuggestionsProvider(_provider);
            this.ProgressBar_Search.Visibility = Visibility.Collapsed;
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

        private void RadAutoCompleteBox_Search_LostFocus(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        private void AppBarButton_Back_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.GoBack();
        }

        private void AppBarButton_Forward_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.Navigate(typeof(PageImWithGirlsAndGuys));
        }

        private void AppBarButton_Leave_Click(object sender, RoutedEventArgs e)
        {
            App.QuitFromEditPost();
        }


    }
}
