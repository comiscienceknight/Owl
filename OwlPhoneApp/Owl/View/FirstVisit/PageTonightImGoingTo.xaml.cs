using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Owl.View.FirstVisit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageTonightImGoingTo : Page
    {
        public PageTonightImGoingTo()
        {
            this.InitializeComponent();
            this.Loaded += PageTonightImGoingTo_Loaded;
        }

        void PageTonightImGoingTo_Loaded(object sender, RoutedEventArgs e)
        {
            if(App.MyPost != null)
            {
                if(App.MyPost.VenueId != null)
                {
                    RadioButton_Venue.IsChecked = true;
                    RadioButton_Anywhere.IsChecked = false;
                }
            }
            else
            {
                App.MyPost = new DataObjects.Post();
                App.MyPost.UserId = App.MySelf.UserId;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(App.MyPost == null)
            {
                App.MyPost = new DataObjects.Post();
                App.MyPost.UserId = App.MySelf.UserId;
                App.MyPost.UserName = App.MySelf.UserName;
                App.MyPost.UserPopularity = App.MySelf.Popularity;
            }
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
            App.QuitFromEditPost();
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
