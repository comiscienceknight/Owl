using GalaSoft.MvvmLight.Messaging;
using Microsoft.Xaml.Interactivity;
using OwlWindowsPhoneApp.DataObjects;
using OwlWindowsPhoneApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public sealed partial class MessagePage : Page
    {
        private string _userId;
        private string _userName;
        private string _userProfileUrl;

        public MessagePage()
        {
            this.InitializeComponent();
            this.DataContext = new MessageViewModel();
            this.Loaded += MessagePage_Loaded;
        }

        void MessagePage_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as MessageViewModel).InitBasicInfo(_userId, _userName, _userProfileUrl);
            TextBlock_PageTitle.Text = "Chat - " + _userName;
            Messenger.Default.Register<SendMsgMessage>(this, async msg =>
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
                        ListView_Messages.ScrollIntoView(ListView_Messages.Items.Last() as Message, ScrollIntoViewAlignment.Leading);
                        ProgressBar_Loading.IsIndeterminate = false;
                        ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                });
            });
        }
  
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as ChatEntry;
            if(param != null)
            {
                _userId = param.UserId;
                _userName = param.UserName;
                _userProfileUrl = param.UserProfile;
            }
        }

        private void Button_SendMessage_Click(object sender, RoutedEventArgs e)
        {
        }
    }

    public class ScrollToBottomBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; private set; }

        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object),
            typeof(ScrollToBottomBehavior),
            new PropertyMetadata(null, ItemsSourcePropertyChanged));

        private static void ItemsSourcePropertyChanged(object sender,
            DependencyPropertyChangedEventArgs e)
        {
            var behavior = sender as ScrollToBottomBehavior;
            if (behavior.AssociatedObject == null || e.NewValue == null) return;

            var collection = behavior.ItemsSource as INotifyCollectionChanged;
            if (collection != null)
            {
                collection.CollectionChanged += (s, args) =>
                {
                    //var scrollViewer = behavior.AssociatedObject
                    //                           .GetFirstDescendantOfType<ScrollViewer>();
                    //scrollViewer.ChangeView(null, scrollViewer.ActualHeight, null);
                };
            }
        }

        public void Attach(DependencyObject associatedObject)
        {
            var control = associatedObject as ListView;
            if (control == null)
                throw new ArgumentException(
                    "ScrollToBottomBehavior can be attached only to ListView.");

            AssociatedObject = associatedObject;
        }

        public void Detach()
        {
            AssociatedObject = null;
        }
    }
}
