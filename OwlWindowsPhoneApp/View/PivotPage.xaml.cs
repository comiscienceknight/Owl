using GalaSoft.MvvmLight.Messaging;
using OwlWindowsPhoneApp.Common;
using OwlWindowsPhoneApp.DataObjects;
using OwlWindowsPhoneApp.ViewModel;
using OwlWindowsPhoneApp.ViewModel.Message;
using System;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OwlWindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PivotPage : Page, IFileOpenPickerContinuable
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

            //this.NavigationCacheMode = NavigationCacheMode.Required;

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += this.NavigationHelper_LoadState;
            _navigationHelper.SaveState += this.NavigationHelper_SaveState;

            _geoLocation = new Common.GeoLocation(this.Dispatcher);

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            Messenger.Default.Register<NavigateToPostInfoMessage>(this, msg =>
            {
                NavigateToPostInfoPage(msg.Post);
            });
            Messenger.Default.Register<NavigateToChatMessage>(this, msg =>
            {
                NavigateToMessagePage(msg.ChatEntry.UserId, msg.ChatEntry.UserName, msg.ChatEntry.UserProfile);
            });
            Messenger.Default.Register<NavigateToCameraMessage>(this, msg =>
            {
                NavigateToCameraPhotoPage();
            });
            Messenger.Default.Register<TakePhotoToMyPostMessage>(this, msg =>
            {
                UpdateMyPostPhoto(msg.BitMap);
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
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

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }


        #region Menu items and hardwar back button click
        private async void AppBarButton_Logout_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Do you want to logout?");
            dialog.Commands.Add(new UICommand("YES"));
            dialog.Commands.Add(new UICommand("NO"));
            var returnCommand = await dialog.ShowAsync();
            if(returnCommand.Label == "YES")
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
        }

        private void AppBarButton_Message_Click(object sender, RoutedEventArgs e)
        {
            if (Grid_SubPage.Children.Count > 0 && Grid_SubPage.Children.First() is PostInfoUserControl)
            {
                var postInfoUC = Grid_SubPage.Children.First() as PostInfoUserControl;
                Post post = postInfoUC.GetPost();
                Grid_SubPage.Children.Clear();
                NavigateToMessagePage(post.UserId, post.UserName, post.ProfileUrl);
            }
        }

        private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Grid_SubPage.Children.Any(p => p is PostInfoUserControl))
            {
                Grid_SubPage.Children.Clear();
                Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                UpdateAppBarItems(Pivot_Main.SelectedItem as PivotItem);
            }
            else if (Grid_SubPage.Children.Any(p => p is MessageUserControl))
            {
                Grid_SubPage.Children.Clear();
                Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                UpdateAppBarItems(Pivot_Main.SelectedItem as PivotItem);
            }
            else if (Grid_SubPage.Children.Any(p => p is CameraPhotoUserControl))
            {
                CameraPhotoUserControl uc = Grid_SubPage.Children.First() as CameraPhotoUserControl;
                if(uc.GetImageEffectsUserControl() != null)
                {
                    uc.UnloadImageEffectsUserControl();
                }
                else
                {
                    Grid_SubPage.Children.Clear();
                    Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    UpdateAppBarItems(Pivot_Main.SelectedItem as PivotItem);
                }
            }
            else if (e.Handled == false)
            {
                e.Handled = true;
                var dialog = new MessageDialog("Do you want quit app Owl?");
                dialog.Commands.Add(new UICommand("YES"));
                dialog.Commands.Add(new UICommand("NO"));
                var returnCommand = await dialog.ShowAsync();
                if (returnCommand.Label == "YES")
                {
                    Application.Current.Exit();
                }
            }
        }
        #endregion


        #region internal navigation
        private void NavigateToMessagePage(string userId, string userName, string profileUrl)
        {
            Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Grid_SubPage.Children.Clear();
            Grid_SubPage.Children.Add(new MessageUserControl(userId, userName, profileUrl));
            AppBarButton_FilterPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AppBarButton_RefreshPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void NavigateToPostInfoPage(Post post = null)
        {
            Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Grid_SubPage.Children.Clear();
            if(post != null)
            {
                Grid_SubPage.Children.Add(new PostInfoUserControl(post));
                AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Visible;
                AppBarButton_FilterPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                AppBarButton_RefreshPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void UpdateMyPostPhoto(RenderTargetBitmap bmp)
        {
            CameraPhotoUserControl uc = Grid_SubPage.Children.First() as CameraPhotoUserControl;
            if (uc.GetImageEffectsUserControl() != null)
            {
                uc.UnloadImageEffectsUserControl();
            }
            Grid_SubPage.Children.Clear();
            Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            UpdateAppBarItems(Pivot_Main.SelectedItem as PivotItem);
            UserControl_MyPost.ChangeImageProfile1(bmp);
        }

        private void NavigateToCameraPhotoPage()
        {
            Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Grid_SubPage.Children.Clear();
            var cameraView = new CameraPhotoUserControl();
            Grid_SubPage.Children.Add(new CameraPhotoUserControl());
            AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AppBarButton_FilterPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AppBarButton_RefreshPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

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
                    case "Posts":
                        AppBarButton_FilterPost.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        AppBarButton_RefreshPost.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        break;
                    default:
                        AppBarButton_FilterPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_RefreshPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        break;
                }
            }
        }
        #endregion

        public void ContinueFileOpenPicker(Windows.ApplicationModel.Activation.FileOpenPickerContinuationEventArgs args)
        {
            var file = args.Files.FirstOrDefault();
            if (file == null)
                return;

            if(Grid_SubPage.Children != null && Grid_SubPage.Children.Count > 0)
            {
                var uc = Grid_SubPage.Children.First() as CameraPhotoUserControl;
                if(uc != null)
                {
                    uc.OpenImagePreview(file);
                }
            }
        }
    }
}
