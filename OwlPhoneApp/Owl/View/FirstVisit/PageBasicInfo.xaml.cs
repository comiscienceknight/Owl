using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

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
            this.Loaded += PageBasicInfo_Loaded;
        }

        async void PageBasicInfo_Loaded(object sender, RoutedEventArgs e)
        {
            Border_Root.Visibility = Windows.UI.Xaml.Visibility.Visible;
            _mySelf = await GetUser();
            Border_Root.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            EnableSubmit();

            if (App.MySelf == null)
            {
                App.MySelf = _mySelf;
                if (!string.IsNullOrWhiteSpace(_mySelf.UserId))
                {
                    var rootFrame = (Window.Current.Content as Frame);
                    rootFrame.Navigate(typeof(PageTonightImGoingTo));
                }
            }

        }


        #region get User from server
        public async Task<DataObjects.User> GetUser()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                var user = await httpClient.GetStringAsync(
                    new Uri("http://owlbat.azure-mobile.net/get/getuser/" + App.UserId));

                JsonValue jsonValue = JsonValue.Parse(user);
                return AnalyseUserJsonValueArray(jsonValue.GetObject());
            }
        }

        private DataObjects.User AnalyseUserJsonValueArray(IJsonValue userJsonValue)
        {
            JsonObject jo = userJsonValue.GetObject();
            var user = new DataObjects.User();
            if (jo.ContainsKey("id"))
            {
                user.Id = jo.GetNamedString("id");
                if (jo.ContainsKey("userName"))
                    user.UserName = jo.GetNamedString("userName");
                if (jo.ContainsKey("userId"))
                    user.UserId = jo.GetNamedString("userId");
                if (jo.ContainsKey("sexe"))
                    user.Sexe = jo.GetNamedString("sexe");
                if (jo.ContainsKey("birthday"))
                {
                    string birthday = jo.GetNamedString("birthday");
                    if(birthday.Length == 10)
                    user.Birthday = new DateTimeOffset(
                        new DateTime(Convert.ToInt32(birthday.Substring(0,4)), Convert.ToInt32(birthday.Substring(5,2)), Convert.ToInt32(birthday.Substring(8,2))));
                }
            }
            else
            {
                user.UserId = App.UserId;
            }
            return user;
        }
        #endregion


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
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

        private async void Button_Submit_Click(object sender, RoutedEventArgs e)
        {
            _mySelf.UserId = App.UserId;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                var prms = new Dictionary<string, string>();
                prms.Add("sexe", _mySelf.Sexe);
                prms.Add("userid", _mySelf.UserId);
                prms.Add("username", _mySelf.UserName);
                prms.Add("birthday", _mySelf.Birthday.ToString("yyyy-MM-dd"));

                HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(prms);
                HttpResponseMessage response = await client.PostAsync(new Uri("http://owlbat.azure-mobile.net/post/createuser"), formContent);
                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync();

                if (response.Content != null && response.Content.ToString() != "")
                {
                    var dialog = new MessageDialog(response.Content.ToString());
                    await dialog.ShowAsync();
                }
            }

            var rootFrame = (Window.Current.Content as Frame);
            rootFrame.Navigate(typeof(PageTonightImGoingTo));
        }
    }
}
