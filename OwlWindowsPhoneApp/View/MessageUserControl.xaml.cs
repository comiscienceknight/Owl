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
            this.Loaded += MessageUserControl_Loaded;
        }

        void MessageUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock_PageTitle.Text = "Owl Message - " + _userName;
        }
    }
}
