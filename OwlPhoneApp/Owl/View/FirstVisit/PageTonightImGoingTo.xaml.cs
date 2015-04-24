using Owl.Models;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Owl.View.FirstVisit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageTonightImGoingTo : Page
    {
        private bool _purposeForUpdating = false;

        public PageTonightImGoingTo()
        {
            this.InitializeComponent();
            this.Loaded += PageTonightImGoingTo_Loaded;
        }

        private async void PageTonightImGoingTo_Loaded(object sender, RoutedEventArgs e)
        {
            Border_Root.Visibility = Windows.UI.Xaml.Visibility.Visible;
            App.MyPost = await JsonReceiver.GetPostByUserId(App.UserId);
            if (string.IsNullOrWhiteSpace(App.MyPost.UserId))
                App.MyPost.UserId = App.MySelf.UserId;
            Border_Root.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            InitUiContentsWithAppMyPost();

            if(_purposeForUpdating)
            {
                AppBarButton_Leave.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                if(!string.IsNullOrWhiteSpace(App.MyPost.Id))
                {
                    var rootFrame = (Window.Current.Content as Frame);
                    rootFrame.Navigate(typeof(PivotPage));
                }
            }
        }

        private void InitUiContentsWithAppMyPost()
        {
            if (!string.IsNullOrWhiteSpace(App.MyPost.VenueId))
            {
                AffectRadioButtons(true, false, false);
            }
            else
            {
                AffectRadioButtons(false, true, false);
            }
        }

        private void AffectRadioButtons(bool venue, bool anywhere, bool neighborhood)
        {
            RadioButton_Venue.IsChecked = venue;
            RadioButton_Anywhere.IsChecked = anywhere;
            RadioButton_Neighborhood.IsChecked = neighborhood;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is bool)
                _purposeForUpdating = (bool)e.Parameter;
        }

        private void RadioButton_Venue_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Neighborhood_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void RadioButton_Anywhere_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void AppBarButton_Leave_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.GoBack();
        }

        private void AppBarButton_Forward_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            if (RadioButton_Venue.IsChecked == true)
            {
                rootFrame.Navigate(typeof(PageTonightImGoingToSearchVenues));
            }
            else if (RadioButton_Neighborhood.IsChecked == true)
            {
                rootFrame.Navigate(typeof(PageTonightImGoingToSearchVenues));
            }
            else if (RadioButton_Anywhere.IsChecked == true)
            {
                App.MyPost.VenueId = null;
                App.MyPost.Place = "Anywhere";
                rootFrame.Navigate(typeof(PageImWithGirlsAndGuys));
            }
        }
    }
}
