using GalaSoft.MvvmLight.Messaging;
using Microsoft.WindowsAzure.MobileServices;
using OwlWindowsPhoneApp.Common;
using OwlWindowsPhoneApp.DataObjects;
using OwlWindowsPhoneApp.ViewModel;
using System;
using System.Linq;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.Security.Credentials;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace OwlWindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer _dispatcherTimer = null;
        private int _syncObjBool = 0;

        private NavigationHelper _navigationHelper;
        public NavigationHelper NavigationHelper
        {
            get { return this._navigationHelper; }
        }

        public MainPage()
        {
            this.InitializeComponent();
               
            //this.NavigationCacheMode = NavigationCacheMode.Required;
            
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += this.NavigationHelper_LoadState;
            _navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.NavigationParameter.ToString() == "logout")
            {
                ShowAllLoginButtons();
            }
            else
            {
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
                _dispatcherTimer.Tick += DispatcherTimer_Tick;
                _dispatcherTimer.Start();

                HideAllLoginButtons();
            }
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private async void DispatcherTimer_Tick(object sender, object e)
        {
            if (Interlocked.CompareExchange(ref _syncObjBool, 1, 0) == 0)
            {
                try
                {
                    if(!TryAuthenticateWithStoredToken())
                    {
                        ShowAllLoginButtons();
                    }
                }
                finally
                {
                    Interlocked.CompareExchange(ref _syncObjBool, 0, 1);
                }
            }
        }

        private void HideAllLoginButtons()
        {
            ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ProgressBar_Loading.IsIndeterminate = true;
            StackPanel_SignIn.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void ShowAllLoginButtons()
        {
            ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ProgressBar_Loading.IsIndeterminate = false;
            StackPanel_SignIn.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private bool TryAuthenticateWithStoredToken(string provider = "MicrosoftAccount", 
            MobileServiceAuthenticationProvider providerEnum = MobileServiceAuthenticationProvider.MicrosoftAccount)
        {
            try
            {
                // Use the PasswordVault to securely store and access credentials.
                PasswordVault vault = new PasswordVault();
                // Try to get an existing credential from the vault.
                PasswordCredential credential = vault.FindAllByResource(provider).FirstOrDefault();
                if (credential != null)
                {
                    // Create a user from the stored credentials.
                    MobileServiceUser user = new MobileServiceUser(credential.UserName);
                    credential.RetrievePassword();
                    user.MobileServiceAuthenticationToken = credential.Password;

                    // Set the user from the stored credentials.
                    App.OwlbatClient.CurrentUser = user;
                    App.PasswordVaultObject = vault;

                    _dispatcherTimer.Stop();
                    
                    var rootFrame = (Window.Current.Content as Frame);
                    if (!rootFrame.Navigate(typeof(PivotPage)))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                // When there is no matching resource an error occurs, which we ignore.
            }
            return false;
        }

        private async System.Threading.Tasks.Task AuthenticateAsync(string provider = "MicrosoftAccount",
            MobileServiceAuthenticationProvider providerEnum = MobileServiceAuthenticationProvider.MicrosoftAccount)
        {
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;
            var loginAsync = App.OwlbatClient.LoginAsync(providerEnum);
            try
            {
                // Login with the identity provider.
                MobileServiceUser user = await loginAsync;
                // Create and store the user credentials.
                credential = new PasswordCredential(provider,
                    user.UserId, user.MobileServiceAuthenticationToken);
                vault.Add(credential);
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                return;
            }
            catch (Exception ex)
            {
                return;
            }
            finally
            {
            }
            HideAllLoginButtons();
            App.PasswordVaultObject = vault;

            var rootFrame = (Window.Current.Content as Frame);
            if (!rootFrame.Navigate(typeof(PivotPage)))
            {
                throw new Exception("Failed to create initial page");
            }
        }

        private async void Image_HotmailSignIn_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (Interlocked.CompareExchange(ref _syncObjBool, 1, 0) == 0)
            {
                HideAllLoginButtons();
                try
                {
                    await AuthenticateAsync();
                }
                finally
                {
                    Interlocked.CompareExchange(ref _syncObjBool, 0, 1);
                }
            }
        }
    }
}
