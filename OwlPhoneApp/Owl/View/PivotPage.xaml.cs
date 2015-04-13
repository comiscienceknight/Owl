using GalaSoft.MvvmLight.Messaging;
using Owl;
using OwlWindowsPhoneApp.Common;
using OwlWindowsPhoneApp.DataObjects;
using OwlWindowsPhoneApp.View;
using OwlWindowsPhoneApp.ViewModel;
using OwlWindowsPhoneApp.ViewModel.Message;
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
            this.Loaded += PivotPage_Loaded;

            Messenger.Default.Register<NavigateToPostInfoMessage>(this, msg =>
            {
                NavigateToPostInfoPage(msg.Post);
            });
            Messenger.Default.Register<NavigateToChatMessage>(this, msg =>
            {
                NavigateToMessagePage(msg.ChatEntry.UserId, msg.ChatEntry.UserName, msg.ChatEntry.UserProfile);
            });
        }

        void PivotPage_Loaded(object sender, RoutedEventArgs e)
        {
            Grid_SubPage.Children.Clear();
            AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        void FirstTimeUc_TakePhotoClick(object sender, EventArgs e)
        {
            Grid_SubPage.Children.Clear();
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.PickSingleFileAndContinue();
        }

        #region Take Photo
        private void UserControl_MyPost_Loaded(object sender, RoutedEventArgs e)
        {
            //UserControl_MyPost.TakePhotoEvent += UserControl_MyPost_TakePhotoEvent;
            UserControl_MyPost.SetPostByUserId();
        }

        //void UserControl_MyPost_TakePhotoEvent(object sender, ProfilePhotoClickEventArg e)
        //{
        //    Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Visible;
        //    Grid_SubPage.Children.Clear();
        //    var cameraView = new CameraPhotoUserControl();
        //    cameraView.ProfileNumber = e.ProfilePhotoNumber;
        //    cameraView.ChoosePhotoFromStorageEvent += CameraView_ChoosePhotoFromStorageEvent;
        //    cameraView.TakePhotoEvent += CameraView_TakePhotoEvent;
        //    Grid_SubPage.Children.Add(cameraView);
        //    AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        //    AppBarButton_FilterPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        //    AppBarButton_RefreshPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        //    AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        //    AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        //}

        //void CameraView_TakePhotoEvent(object sender, TakePhotoClickEventArg e)
        //{
        //    ((CameraPhotoUserControl)sender).TakePhotoEvent -= CameraView_TakePhotoEvent;
        //    ((CameraPhotoUserControl)sender).ChoosePhotoFromStorageEvent -= CameraView_ChoosePhotoFromStorageEvent;
        //    Grid_SubPage.Children.Clear();
        //    var imageEffectUserControl = new ImageEffectsUserControl(e.TakedPhotoImage, e.TakedPhotoFile, e.TakedPhotoFile.Path)
        //    {
        //        ProfileNumber = e.ProfilePhotoNumber
        //    };
        //    imageEffectUserControl.ProfilePhotoRendered += ImageEffectUserControl_ProfilePhotoRendered;
        //    Grid_SubPage.Children.Add(imageEffectUserControl);
        //}

        //void ImageEffectUserControl_ProfilePhotoRendered(object sender, ProfilePhotoRenderedEventArg e)
        //{
        //    ((ImageEffectsUserControl)sender).ProfilePhotoRendered -= ImageEffectUserControl_ProfilePhotoRendered;
        //    UserControl_MyPost.ChangeImageProfile(e.TakedPhotoImage, e.ProfilePhotoNumber);
        //    Grid_SubPage.Children.Clear();
        //    Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        //    UpdateAppBarItems(Pivot_Main.SelectedItem as PivotItem);
        //}

        //void CameraView_ChoosePhotoFromStorageEvent(object sender, ChoosePhotoFromStorageClickEventArg e)
        //{
        //    ((CameraPhotoUserControl)sender).TakePhotoEvent -= CameraView_TakePhotoEvent;
        //    ((CameraPhotoUserControl)sender).ChoosePhotoFromStorageEvent -= CameraView_ChoosePhotoFromStorageEvent;
        //    Grid_SubPage.Children.Clear();

        //    FileOpenPicker openPicker = new FileOpenPicker();
        //    openPicker.ViewMode = PickerViewMode.Thumbnail;
        //    openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        //    openPicker.FileTypeFilter.Add(".jpg");
        //    openPicker.FileTypeFilter.Add(".jpeg");
        //    openPicker.FileTypeFilter.Add(".png");
        //    openPicker.PickSingleFileAndContinue();
        //}

        public async void ContinueFileOpenPicker(Windows.ApplicationModel.Activation.FileOpenPickerContinuationEventArgs args)
        {
            //if (args.Files.Count > 0)
            //{
            //    var file = args.Files.FirstOrDefault();
            //    if (file == null)
            //        return;

            //    if (Grid_SubPage.Children != null && Grid_SubPage.Children.Count > 0)
            //    {
            //        Grid_SubPage.Children.Clear();
            //    }
            //    StorageFile sf = args.Files.First();
            //    BitmapImage bmpImage = await CameraPhotoUserControl.LoadImage(sf);
            //    ImageEffectsUserControl imageEffectUc = new ImageEffectsUserControl(bmpImage, sf, sf.Path);
            //    imageEffectUc.ProfilePhotoRendered += ImageEffectUserControl_ProfilePhotoRendered;
            //    Grid_SubPage.Children.Add(imageEffectUc);
            //}
            //else
            //{
            //    if (Grid_SubPage.Children.First() is FirstTimeEnterUserControl)
            //    {

            //    }
            //    else
            //    {
            //        Grid_SubPage.Children.Clear();
            //        UpdateAppBarItems(Pivot_Main.SelectedItem as PivotItem);
            //    }
            //}
        }
        #endregion

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

        private void AppBarButton_EditProfile_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            if (!rootFrame.Navigate(typeof(BlankPage1)))
            {
                throw new Exception("Failed to create MainPage");
            }
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
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
                Grid_SubPage.Children.Clear();
                Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                UpdateAppBarItems(Pivot_Main.SelectedItem as PivotItem);
            }
            else if (Grid_SubPage.Children.Any(p => p is ImageEffectsUserControl))
            {
                Grid_SubPage.Children.Clear();

                Grid_SubPage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                UpdateAppBarItems(Pivot_Main.SelectedItem as PivotItem);
            }
            else if (e.Handled == false)
            {
                e.Handled = true;
                QuitApp();
            }
        }

        private async void QuitApp()
        {
            var dialog = new MessageDialog("Do you want quit app Owl?");
            dialog.Commands.Add(new UICommand("YES"));
            dialog.Commands.Add(new UICommand("NO"));
            var returnCommand = await dialog.ShowAsync();
            if (returnCommand.Label == "YES")
            {
                Application.Current.Exit();
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
                        AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        AppBarButton_EditProfile.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        break;
                    case "me":
                        AppBarButton_FilterPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_RefreshPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_EditProfile.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        break;
                    default:
                        AppBarButton_FilterPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_RefreshPost.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_Message.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_EditProfile.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        AppBarButton_Logout.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        AppBar_Pivot.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        break;
                }
            }
        }
        #endregion
    }
}
