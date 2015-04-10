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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OwlWindowsPhoneApp
{
    public sealed partial class FirstTimeEnterUserControl : UserControl
    {
        public event EventHandler<EventArgs> GuideFinished;
        public event EventHandler<EventArgs> TakePhotoClick;
        /// <summary>
        /// 1 for man, 0 for woman
        /// </summary>
        private bool? _maleOrFemale = null;

        private double _unitOffset = 0;

        public FirstTimeEnterUserControl()
        {
            this.InitializeComponent();

            this.Loaded += FirstTimeEnterUserControl_Loaded;
        }

        void FirstTimeEnterUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitAutoTextComplete();

            _unitOffset = Window.Current.Bounds.Width;
            ScrollViewer_Main.Height = Window.Current.Bounds.Width;
            StackPanel_ScrollViewer.Height = Window.Current.Bounds.Width;
            StackPanel_ScrollViewer.Width = Window.Current.Bounds.Width * 6 - 20; 
            Grid_PickProfile.Width = Window.Current.Bounds.Width - 20;
            Grid_Iam.Width = Grid_PickProfile.Width;
            StackPanel_AgeName.Width = Grid_PickProfile.Width;
            Grid_SearchVenue.Width = Grid_PickProfile.Width;
            Grid_NumberOfGroup.Width = Grid_PickProfile.Width;
            Grid_Description.Width = Grid_PickProfile.Width;

            ScrollViewer_Main.IsDeferredScrollingEnabled = true;

            ListPickerFlyout_AgeRange.ItemsSource = new List<string>() { "avcdeff", "abfdfe" };
            ListPickerFlyout_Description.ItemsSource = new List<string>() { 
                "Hi, I wanna have fun and go to some night club. But i don't have wings. Any girl can take me into the bar or club, i'm glad to invite her a drink", 
                "Hi, I wanna have fun and go to some night club. But i don't have wings. Any girl can take me into the bar or club, i'm glad to invite her a drink",
                "Hi, I wanna have fun and go to some night club. But i don't have wings. Any girl can take me into the bar or club, i'm glad to invite her a drink",
                "Hi, I wanna have fun and go to some night club. But i don't have wings. Any girl can take me into the bar or club, i'm glad to invite her a drink"
            };

            ScrollViewer_Main.ViewChanged += ScrollViewer_Main_ViewChanged;
        }

        void ScrollViewer_Main_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if((int)(ScrollViewer_Main.HorizontalOffset / _unitOffset) == 5)
            {
                TextBlock_Indication.Text = "Start";
                Rectangle_Indication.Opacity = 1;
            }
        }

        private async void TextBlock_Indication_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(TextBlock_Indication.Text == "Start")
            {
                if (GuideFinished != null)
                    GuideFinished(this, new EventArgs());
            }
            else
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock_Indication.Text = "";
                    Rectangle_Indication.Opacity = 0;
                    TextBlock_VenueName.Text = RadAutoCompleteBox_Search.Text;
                    ScrollToNext();
                });
            }
        }

        private void ScrollToNext()
        {
            int offsetBrick = (int)(ScrollViewer_Main.HorizontalOffset / _unitOffset);
            ScrollViewer_Main.ChangeView((offsetBrick % 6) * _unitOffset + _unitOffset, null, null, false);
        }

        private async void Image_Profile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (TakePhotoClick != null)
                {
                    TakePhotoClick(this, null);
                }
            });
        }

        public async void ChangeImageProfile(RenderTargetBitmap bmp)
        {
            if (bmp != null)
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock_Indication.Text = "Next";
                    Rectangle_Indication.Opacity = 1;
                    Image_Profile.Source = bmp;
                    TextBlock_TouchMyWings.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                });
            }
        }

        private async void Image_Boy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextBlock_HiSex.Text = "My Gentleman!";
                _maleOrFemale = true; 
                ScrollToNext();
            });
        } 

        private async void Image_Girls_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextBlock_HiSex.Text = "Hi Lady!";
                _maleOrFemale = false;
                ScrollToNext();
            });
        }

        private async void NumericUpDown_WithBoys_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if(e.NewValue > 0)
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock_Indication.Text = "Next";
                    Rectangle_Indication.Opacity = 1;
                });
            }
        }

        private async void NumericUpDown_WithGirls_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue > 0)
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock_Indication.Text = "Next";
                    Rectangle_Indication.Opacity = 1;
                });
            }
        }

        private async void TextBox_Description_TextChanged(object sender, RoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (ScrollViewer_Main.HorizontalOffset > 2000)
                {
                    TextBlock_Indication.Text = "Start";
                    Rectangle_Indication.Opacity = 1;
                }
            });
        }

        private async void TextBox_NickName_TextChanged(object sender, TextChangedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextBlock_Indication.Text = "Next";
                Rectangle_Indication.Opacity = 1;
            });
        }

        private async void TextBox_AgeRange_TextChanged(object sender, TextChangedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextBlock_Indication.Text = "Next";
                Rectangle_Indication.Opacity = 1;
            });
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
                    TextBlock_Indication.Text = "Next";
                    Rectangle_Indication.Opacity = 1;
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
            this.LostFocusButton.Focus(FocusState.Pointer);
        }
        #endregion
    }

    public class SearchAvenues
    {
        public string VenueId { get; set; }
        public string Venue { get; set; }
        public string Adresse { get; set; }
    }
}
