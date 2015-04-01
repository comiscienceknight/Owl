using OwlWindowsPhoneApp.DataObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OwlWindowsPhoneApp
{
    public sealed partial class PostsUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<Post> _postCollection = new ObservableCollection<Post>();
        public ObservableCollection<Post> PostCollection
        {
            get { return _postCollection; }
            set
            {
                _postCollection = value;
                OnPropertyChanged("PostCollection");
            }
        }
        

        public PostsUserControl()
        {
            this.InitializeComponent();
            this.Loaded += PostsUserControl_Loaded;
        }

        void PostsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ListView_Posts.ItemsSource = PostCollection;
            LoadPosts();
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dialog = new MessageDialog(e.ClickedItem.ToString());
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }

        private async void LoadPosts()
        {
            ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ProgressBar_Loading.IsIndeterminate = true;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                var posts = await httpClient.GetStringAsync(
                    new Uri("http://owlbat.azure-mobile.net/get/randomposts/Paris/France"));

                JsonValue jsonValue = JsonValue.Parse(posts);
                AnalysePostJsonValueArray(jsonValue);
            }
            ProgressBar_Loading.IsIndeterminate = false;
            ProgressBar_Loading.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void AnalysePostJsonValueArray(JsonValue postsJasonValue)
        {
            if(postsJasonValue.ValueType == JsonValueType.Array)
            {
                foreach(var jv in postsJasonValue.GetArray().ToList())
                {
                    PostCollection.Add(AnalysePostJsonValue(jv));
                }
            }
        }

        private DataObjects.Post AnalysePostJsonValue(IJsonValue postJasonValue)
        {
            JsonObject jo = postJasonValue.GetObject(); 
            var post = new DataObjects.Post();

            post.Place = jo.GetNamedString("placeName");

            if (jo.ContainsKey("description"))
                post.Description = jo.GetNamedString("description");

            if (jo.ContainsKey("profileUrl"))
            {
                Uri myUri = new Uri(jo.GetNamedString("profileUrl"), UriKind.Absolute);
                BitmapImage bmi = new BitmapImage();
                bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmi.UriSource = myUri;
                post.Profile = bmi;
            }

            if (jo.ContainsKey("girlsNumber"))
                post.GirlsNumber = (int)jo.GetNamedNumber("girlsNumber");

            if (jo.ContainsKey("guysNumber"))
                post.GuysNumber = (int)jo.GetNamedNumber("guysNumber");

            if (jo.ContainsKey("time"))
                post.Time = jo.GetNamedString("time");

            post.UserId = jo.GetNamedString("userId");

            post.Require = string.Format("+ {0} boys, + {1} girls. {2}", post.GuysNumber, post.GirlsNumber, post.Time);

            return post;
        }
    }
}
