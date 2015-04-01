using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Linq;
using System.Threading;
using Windows.Security.Credentials;
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

        public MainPage()
        {
            this.InitializeComponent();
               
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();
        }

        async void DispatcherTimer_Tick(object sender, object e)
        {
            if (Interlocked.CompareExchange(ref _syncObjBool, 1, 0) == 0)
            {
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

        private async void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            // Login the user and then load data from the mobile service.
            await AuthenticateAsync();
        }

        private async System.Threading.Tasks.Task AuthenticateAsync()
        {
            ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ProgressBar_Loading.IsIndeterminate = true;
            this.Button_Login.IsEnabled = false;

            // This sample uses the MicrosoftAccount provider.
            var provider = "MicrosoftAccount";
            MobileServiceUser user = null;
            this.Button_Login.IsEnabled = false;

            string message;
            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;
            try
            {
                // Try to get an existing credential from the vault.
                credential = vault.FindAllByResource(provider).FirstOrDefault();
            }
            catch (Exception)
            {
                // When there is no matching resource an error occurs, which we ignore.
            }
            if (credential != null)
            {
                // Create a user from the stored credentials.
                user = new MobileServiceUser(credential.UserName);
                credential.RetrievePassword();
                user.MobileServiceAuthenticationToken = credential.Password;

                // Set the user from the stored credentials.
                App.OwlbatClient.CurrentUser = user;
            }
            else
            {
                try
                {
                    // Login with the identity provider.
                    user = await App.OwlbatClient
                        .LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
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
                    this.Button_Login.IsEnabled = true;
                    this.Button_Login.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    _dispatcherTimer.Stop();
                    ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    ProgressBar_Loading.IsIndeterminate = false;
                }
            }
            this.Button_Login.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            _dispatcherTimer.Stop();
            ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ProgressBar_Loading.IsIndeterminate = false;

            var rootFrame = (Window.Current.Content as Frame);
            if (!rootFrame.Navigate(typeof(PivotPage)))
            {
                throw new Exception("Failed to create initial page");
            }
        }

    }
}
