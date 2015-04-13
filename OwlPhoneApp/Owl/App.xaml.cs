using Microsoft.WindowsAzure.MobileServices;
using OwlWindowsPhoneApp.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace OwlWindowsPhoneApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        // http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409
        public static Microsoft.WindowsAzure.MobileServices.MobileServiceClient owlbatClient = new Microsoft.WindowsAzure.MobileServices.MobileServiceClient(
        "https://owlbat.azure-mobile.net/",
        "FeeTSGBvxVAVzirOfnjTWHAKBRfvFV37");

        // http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409
        public static Microsoft.WindowsAzure.MobileServices.MobileServiceClient OwlbatClient = new Microsoft.WindowsAzure.MobileServices.MobileServiceClient(
        "https://owlbat.azure-mobile.net/",
        "FeeTSGBvxVAVzirOfnjTWHAKBRfvFV37");
        public static PasswordVault PasswordVaultObject;
        public ContinuationManager ContinuationManager { get; private set; }
        private TransitionCollection _transitions;
        public static string UserId
        {
            get { return App.OwlbatClient.CurrentUser.UserId.Replace(":", ""); }
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this._transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this._transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this._transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            // Windows Phone 8.1 requires you to handle the respose from the WebAuthenticationBroker.
#if WINDOWS_PHONE_APP
            //if (args.Kind == ActivationKind.WebAuthenticationBrokerContinuation)
            //{
            //    // Completes the sign-in process started by LoginAsync.
            //    // Change 'MobileService' to the name of your MobileServiceClient instance. 
            //    App.OwlbatClient.LoginComplete(args as WebAuthenticationBrokerContinuationEventArgs);
            //}
            //else
#endif
            {
                ContinuationManager = new ContinuationManager();

                //await RestoreStatusAsync(args.PreviousExecutionState);

                var continuationEventArgs = args as IContinuationActivatedEventArgs;
                if (continuationEventArgs != null)
                {
                    // Call ContinuationManager to handle continuation activation
                    ContinuationManager.Continue(continuationEventArgs);
                }

                //Window.Current.Activate();
            }
        }

        private async Task RestoreStatusAsync(ApplicationExecutionState previousExecutionState)
        {
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (previousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (SuspensionManagerException)
                {
                    //Something went wrong restoring state.
                    //Assume there is no state and continue
                }
            }
        }
    }
}