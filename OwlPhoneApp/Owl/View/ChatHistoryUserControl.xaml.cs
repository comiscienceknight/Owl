using GalaSoft.MvvmLight.Messaging;
using Microsoft.AspNet.SignalR.Client;
using Owl.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Owl
{
    public sealed partial class ChatHistoryUserControl : UserControl
    {
        public ChatHistoryUserControl()
        {
            this.InitializeComponent();
            this.Loaded += PostsUserControl_Loaded;
            Messenger.Default.Register<LoadingAnimationMessage>(this, LoadingAnimationMessage.ChatToken, async msg =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (msg.IfLoading)
                    {
                        ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        ProgressBar_Loading.IsIndeterminate = true;
                    }
                    else
                    {
                        ProgressBar_Loading.IsIndeterminate = false;
                        ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                });
            });
        }

        void PostsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        HubConnection hubConnection;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnection(App.OwlbatClient.ApplicationUri.AbsoluteUri);
                if (App.OwlbatClient != null)
                {
                    hubConnection.Headers["x-zumo-auth"] = App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken;
                }
                else
                {
                    hubConnection.Headers["x-zumo-application"] = App.OwlbatClient.ApplicationKey;
                }
                IHubProxy proxy = hubConnection.CreateHubProxy("GpsHub");
                await hubConnection.Start();

                string result = await proxy.Invoke<string>("Send", "Hello World!");
                var invokeDialog = new MessageDialog(result);
                await invokeDialog.ShowAsync();

                proxy.On<string>("BoardMyPos", async msg =>
                {
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        var callbackDialog = new MessageDialog(msg);
                        callbackDialog.Commands.Add(new UICommand("OK"));
                        await callbackDialog.ShowAsync();
                    });
                });

                proxy.On<string>("Send", async msg =>
                {
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        var callbackDialog = new MessageDialog(msg);
                        callbackDialog.Commands.Add(new UICommand("OK"));
                        await callbackDialog.ShowAsync();
                    });
                });

                proxy.Subscribe("Send");
                proxy.Subscribe("BoardMyPos");
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                var prms = new Dictionary<string, string>();
                prms.Add("UserId", App.OwlbatClient.CurrentUser.UserId);
                prms.Add("Altitude", "66.666666");
                prms.Add("Longitude", "88.88888888");

                HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(prms);
                HttpResponseMessage response = await client.PostAsync(new Uri("http://owlbat.azure-mobile.net/api/updatepos"), formContent);
                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync();

                if (response.Content != null && response.Content.ToString() != "")
                {
                    var dialog = new MessageDialog(response.Content.ToString());
                    await dialog.ShowAsync();
                }

            }
        }
    }
}
