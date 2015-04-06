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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OwlWindowsPhoneApp
{
    public sealed partial class MessageUserControl : UserControl
    {
        private readonly string _userId;
        private readonly string _userName;
        private readonly string _userProfileUrl;

        public MessageUserControl()
            :this(null, null, null)
        {

        }

        public MessageUserControl(string userId, string userName, string userProfileUrl)
        {
            this.InitializeComponent();
            _userId = userId;
            _userProfileUrl = userProfileUrl;
            _userName = userName;
            this.DataContext = new MessageViewModel(userId, userName, userProfileUrl);
            this.Loaded += MessageUserControl_Loaded;
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

        void MessageUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock_PageTitle.Text = "Chat - " + _userName;
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
