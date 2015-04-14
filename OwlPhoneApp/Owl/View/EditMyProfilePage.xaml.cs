using OwlWindowsPhoneApp.DataObjects;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OwlWindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditMyProfilePage : Page
    {
        private Post _post;

        public EditMyProfilePage()
        {
            this.InitializeComponent();
            this.Loaded += EditMyProfilePage_Loaded;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _post = e.Parameter as Post;
        }

        void EditMyProfilePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        }

        private void AppBarButton_Upload_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.GoBack();
        }
    }
}
