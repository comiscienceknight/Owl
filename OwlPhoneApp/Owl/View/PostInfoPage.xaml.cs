using OwlWindowsPhoneApp;
using OwlWindowsPhoneApp.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace OwlWindowsPhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PostInfoPage : Page
    {
        private Post _post;

        public PostInfoPage()
        {
            this.InitializeComponent();
            this.Loaded += PostInfoPage_Loaded;
        }

        void PostInfoPage_Loaded(object sender, RoutedEventArgs e)
        {
            var postUserControl = new PostInfoUserControl(_post);
            Grid_PostInfo.Children.Add(postUserControl);
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

        private void AppBarButton_Message_Click(object sender, RoutedEventArgs e)
        {
            if (_post != null)
            {
                var rootFrame = (Window.Current.Content as Frame);
                var chatEntry = new ChatEntry()
                        {
                            Time = _post.Time,
                            Message = "",
                            UserId = _post.UserId,
                            UserName = _post.UserName,
                            UserProfile = _post.ProfileUrl + "?Width=125"
                        };
                if (!rootFrame.Navigate(typeof(MessagePage), chatEntry))
                {
                    throw new Exception("Failed to create MainPage");
                }
            }

        }
    }
}
