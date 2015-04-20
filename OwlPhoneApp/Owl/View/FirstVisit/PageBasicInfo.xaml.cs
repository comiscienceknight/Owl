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

namespace Owl.View.FirstVisit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageBasicInfo : Page
    {
        private DataObjects.User _mySelf;

        public PageBasicInfo()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is DataObjects.User)
            {
                _mySelf = e.Parameter as DataObjects.User;
            }
            else
                _mySelf = new DataObjects.User();
            EnableSubmit();
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }

        private void RadioButton_Man_Checked(object sender, RoutedEventArgs e)
        {
            _mySelf.Sexe = "Man";
            EnableSubmit();
        }

        private void RadioButton_Woman_Checked(object sender, RoutedEventArgs e)
        {
            _mySelf.Sexe = "Woman";
            EnableSubmit();
        }

        private void TextBox_UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            _mySelf.UserName = TextBox_UserName.Text;
            EnableSubmit();
        }

        private void DatePicker_Birthday_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            _mySelf.Birthday = DatePicker_Birthday.Date;
            EnableSubmit();
        }

        private void EnableSubmit()
        {
            if(!string.IsNullOrWhiteSpace(_mySelf.Sexe) &&
               !string.IsNullOrWhiteSpace(_mySelf.UserName))
            {
                Button_Submit.IsEnabled = true;
            }
        }

        private void Button_Submit_Click(object sender, RoutedEventArgs e)
        {
            _mySelf.UserId = App.UserId;
            App.MySelf = _mySelf;

            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.Navigate(typeof(PageTonightImGoingTo));
        }
    }
}
