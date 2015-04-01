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

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

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

            // Hide the login button and load items from the mobile service.
            this.Button_Login.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async System.Threading.Tasks.Task AuthenticateAsync()
        {
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
                    this.Button_Login.IsEnabled = true;
                    return;
                }
                catch (Exception ex)
                {
                    this.Button_Login.IsEnabled = true;
                    return;
                }
            }
            this.Button_Login.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            _dispatcherTimer.Stop();

            //this.Frame.Navigate(typeof(PivotPage));
            var rootFrame = (Window.Current.Content as Frame);
            if (!rootFrame.Navigate(typeof(PivotPage)))
            {
                throw new Exception("Failed to create initial page");
            }
        }

        //private async void Button_HttpGet_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        HttpClient httpClient = new HttpClient();
        //        httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", _user.MobileServiceAuthenticationToken);
        //        httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
        //        var places = await httpClient.GetStringAsync(
        //            new Uri("http://owlbat.azure-mobile.net/api/Custom"));
        //        ShowDialog(places);
        //        httpClient.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private async void ShowDialog(string message)
        //{
        //    var dialog = new MessageDialog(message);
        //    dialog.Commands.Add(new UICommand("OK"));
        //    await dialog.ShowAsync();
        //}
    }
}
