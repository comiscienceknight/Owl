using GalaSoft.MvvmLight.Messaging;
using Owl;
using Owl.Common;
using Owl.DataObjects;
using Owl.View;
using Owl.ViewModel;
using Owl.ViewModel.Message;
using System;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Owl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PivotPage : Page
    {
        private bool _readyToQuit = false;

        private NavigationHelper _navigationHelper;
        public NavigationHelper NavigationHelper
        {
            get { return this._navigationHelper; }
        }

        private Common.GeoLocation _geoLocation;

        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += this.NavigationHelper_LoadState;
            _navigationHelper.SaveState += this.NavigationHelper_SaveState;

            _geoLocation = new Common.GeoLocation(this.Dispatcher);

            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.Loaded += PivotPage_Loaded;
            this.Unloaded += PivotPage_Unloaded;

            Messenger.Default.Register<NavigateToPostInfoMessage>(this, msg =>
            {
                Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Visible;
                Grid_SubPage.Children.Clear();
                if (msg.Post != null)
                {

                    var rootFrame = (Window.Current.Content as Frame);
                    _readyToQuit = true;
                    rootFrame.Navigate(typeof(PostInfoPage), msg.Post);
                }
            });
            Messenger.Default.Register<NavigateToChatMessage>(this, msg =>
            {
                //var rootFrame = (Window.Current.Content as Frame);
                //_readyToQuit = true;
                //rootFrame.Navigate(typeof(MessagePage), msg.ChatEntry);
            });
        }

        void PivotPage_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        void PivotPage_Loaded(object sender, RoutedEventArgs e)
        {
            Grid_SubPage.Children.Clear();
            AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            UpdateAppBarItems(Pivot_Main.SelectedItem as PivotItem);
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
            _readyToQuit = false;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (_readyToQuit == false)
                e.Cancel = true;
            base.OnNavigatingFrom(e);
        }


        #region Menu items 
        private async void AppBarButton_Logout_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Do you want to logout?");
            dialog.Commands.Add(new UICommand("YES"));
            dialog.Commands.Add(new UICommand("NO"));
            var returnCommand = await dialog.ShowAsync();
            if (returnCommand.Label == "YES")
            {
                App.OwlbatClient.Logout();
                if (App.PasswordVaultObject != null)
                {
                    App.PasswordVaultObject.Remove(
                        App.PasswordVaultObject.RetrieveAll().FirstOrDefault());
                }

                Messenger.Default.Send<LogoutMessage>(new LogoutMessage());

                _readyToQuit = true;

                var rootFrame = (Window.Current.Content as Frame);
                if (!rootFrame.Navigate(typeof(MainPage), "logout"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
        }

        private void AppBarButton_Filter_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            _readyToQuit = true;
            rootFrame.Navigate(typeof(FilterPage));
        }

        private void AppBarButton_RefreshPost_Click(object sender, RoutedEventArgs e)
        {
            //var cacheSize = ((Frame)Parent).CacheSize;
            //((Frame)Parent).CacheSize = 0;
            //((Frame)Parent).CacheSize = cacheSize;
            Messenger.Default.Send<RefreshPostsMessage>(new RefreshPostsMessage());
        }

        private void AppBarButton_MySelf_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            _readyToQuit = true;
            rootFrame.Navigate(typeof(PostInfoPage), App.MyPost);
        }
        #endregion


        #region internal navigation
        private void Pivot_Main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pivot = sender as Pivot;
            if (pivot != null)
                UpdateAppBarItems(pivot.SelectedItem as PivotItem);
        }

        private void UpdateAppBarItems(PivotItem pivotItem)
        {
            if (pivotItem != null)
            {
                switch (pivotItem.Header.ToString())
                {
                    case "posts":
                        AppBarButton_FilterPost.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        AppBarButton_RefreshPost.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        break;
                    default:
                        AppBarButton_FilterPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_RefreshPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        break;
                }
            }
        }
        #endregion
    }
}
