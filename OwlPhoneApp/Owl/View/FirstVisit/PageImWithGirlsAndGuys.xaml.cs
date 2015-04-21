﻿using System;
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

namespace Owl.View.FirstVisit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageImWithGirlsAndGuys : Page
    {
        private List<string> _lookingFor = new List<string>() { "Free drinks", "Help getting in", "Meeting new people", "Suggest something", "I'm just looking" };

        public PageImWithGirlsAndGuys()
        {
            this.InitializeComponent();
            this.Loaded += PageImWithGirlsAndGuys_Loaded;
        }

        void PageImWithGirlsAndGuys_Loaded(object sender, RoutedEventArgs e)
        {
            ListPickerFlyout_LookingFor.ItemsSource = _lookingFor;
            if (!string.IsNullOrWhiteSpace(App.MyPost.LookingFor))
                ListPickerFlyout_LookingFor.SelectedItem = App.MyPost.LookingFor;
            else
            {
                ListPickerFlyout_LookingFor.SelectedItem = "I'm just looking";
            }

            Button_LookingFor.Content = ListPickerFlyout_LookingFor.SelectedItem;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.MyPost != null)
            {
                NumericUpDown_WithGirls.Value = App.MyPost.GirlsNumber ?? 0;
                NumericUpDown_WithBoys.Value = App.MyPost.GuysNumber ?? 0;
                if (!_lookingFor.Contains(App.MyPost.LookingFor) && !string.IsNullOrWhiteSpace(App.MyPost.LookingFor))
                    _lookingFor.Add(App.MyPost.LookingFor);
            }
            else
            {

            }
        }

        private void NumericUpDown_WithBoys_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            EnableNextButton();
        }

        private void NumericUpDown_WithGirls_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            EnableNextButton();
        }

        private void EnableNextButton()
        {
            if (NumericUpDown_WithGirls.Value <= 0 && NumericUpDown_WithBoys.Value <= 0 ||
                (App.MyPost.LookingFor == "Suggest something" &&
                 string.IsNullOrWhiteSpace(TextBox_Suggestion.Text)))
            {
                AppBarButton_Forward.IsEnabled = false;
            }
            else
            {
                AppBarButton_Forward.IsEnabled = true;
            }
        }

        private void ListPickerFlyout_LookingFor_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            if (sender.SelectedValue.ToString() == "Suggest something")
                TextBox_Suggestion.Visibility = Windows.UI.Xaml.Visibility.Visible;
            else
                TextBox_Suggestion.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            App.MyPost.LookingFor = sender.SelectedValue.ToString();
            Button_LookingFor.Content = ListPickerFlyout_LookingFor.SelectedItem;

            EnableNextButton();
        }

        private void TextBox_Suggestion_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.MyPost.LookingFor = TextBox_Suggestion.Text;
            EnableNextButton();
        }

        private void AppBarButton_Forward_Click(object sender, RoutedEventArgs e)
        {
            App.MyPost.GirlsNumber = (int)NumericUpDown_WithGirls.Value;
            App.MyPost.GuysNumber = (int)NumericUpDown_WithBoys.Value;
            App.MyPost.LookingFor = Button_LookingFor.Content.ToString();
            if (CheckBox_ArrivalTime.IsChecked == true)
                App.MyPost.Time = "Around " + TimePicker_ArrivalTime.Time.ToString().Substring(0, 5);
            else
                App.MyPost.Time = TimePicker_ArrivalTime.Time.ToString("hh:mm");
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.Navigate(typeof(PageWereDressed));
        }

        private void AppBarButton_Leave_Click(object sender, RoutedEventArgs e)
        {
            App.QuitFromEditPost();
        }

        private void AppBarButton_Back_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.GoBack();
        }

        private void TimePicker_ArrivalTime_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            
        }

        private void CheckBox_ArrivalTime_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
