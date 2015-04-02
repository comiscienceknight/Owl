using GalaSoft.MvvmLight.Messaging;
using OwlWindowsPhoneApp.ViewModel;
using System;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OwlWindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PivotPage : Page
    {
        private bool _readyToQuit = false;

        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (_readyToQuit == false)
                e.Cancel = true;
            base.OnNavigatingFrom(e);
        }

        private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(e.Handled == false)
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
    }
}
