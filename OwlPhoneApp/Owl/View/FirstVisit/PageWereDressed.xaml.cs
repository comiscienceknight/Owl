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
    public sealed partial class PageWereDressed : Page
    {
        private List<string> _dressCode = new List<string>() { "Casual", "Casual but nice", "Can get into a club", "High-end attire", "I still have time to change" };

        public PageWereDressed()
        {
            this.InitializeComponent();
            this.Loaded += PageWereDressed_Loaded;
        }

        void PageWereDressed_Loaded(object sender, RoutedEventArgs e)
        {
            ListPickerFlyout_DressCode.ItemsSource = _dressCode;
            if (!string.IsNullOrWhiteSpace(App.MyPost.DressCode))
                ListPickerFlyout_DressCode.SelectedItem = App.MyPost.DressCode;
            else
            {
                ListPickerFlyout_DressCode.SelectedItem = "I still have time to change";
            }

            Button_DressCode.Content = ListPickerFlyout_DressCode.SelectedItem;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void ListPickerFlyout_DressCode_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            Button_DressCode.Content = ListPickerFlyout_DressCode.SelectedItem;
            App.MyPost.DressCode = ListPickerFlyout_DressCode.SelectedItem.ToString();
        }

        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.Navigate(typeof(PageProfile));
        }

        private void Button_Goback_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.GoBack();
        }
    }
}
