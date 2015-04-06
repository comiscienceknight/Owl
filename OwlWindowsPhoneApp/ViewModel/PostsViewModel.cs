using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OwlWindowsPhoneApp.DataObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

namespace OwlWindowsPhoneApp.ViewModel
{
    public class PostsViewModel : ViewModelBase
    {
        private ObservableCollection<Post> _postCollection = new ObservableCollection<Post>();
        public ObservableCollection<Post> PostCollection
        {
            get { return _postCollection; }
            set
            {
                _postCollection = value;
                RaisePropertyChanged("PostCollection");
            }
        }

        public RelayCommand<Post> ItemSelectedCommand { get; set; }
        public RelayCommand ListViewPostLoadedCommand { get; set; }

        public PostsViewModel()
        {
            ItemSelectedCommand = new RelayCommand<Post>(post =>
            {
                //var dialog = new MessageDialog(post.Place);
                //dialog.Commands.Add(new UICommand("OK"));
                //await dialog.ShowAsync();
                Messenger.Default.Send<NavigateToPostInfoMessage>(new NavigateToPostInfoMessage(post));
            });
            ListViewPostLoadedCommand = new RelayCommand(() =>
            {
                LoadPosts();
            });
            Messenger.Default.Register<LogoutMessage>(this, msg =>
            {
                PostCollection.Clear();
            });
        }

        private async void LoadPosts()
        {
            Messenger.Default.Send<LoadingAnimationMessage>(new LoadingAnimationMessage(true));
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                    httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                    var posts = await httpClient.GetStringAsync(
                        new Uri("http://owlbat.azure-mobile.net/get/randomposts/Paris/France"));

                    JsonValue jsonValue = JsonValue.Parse(posts);
                    AnalysePostJsonValueArray(jsonValue);
                } 
            }
            catch
            { 
                
            }
            Messenger.Default.Send<LoadingAnimationMessage>(new LoadingAnimationMessage());
        }

        private void AnalysePostJsonValueArray(JsonValue postsJasonValue)
        {
            if (postsJasonValue.ValueType == JsonValueType.Array)
            {
                foreach (var jv in postsJasonValue.GetArray().ToList())
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
                post.ProfileUrl = jo.GetNamedString("profileUrl");
                Uri myUri = new Uri(post.ProfileUrl + "?Width=175", UriKind.Absolute);
                BitmapImage bmi = new BitmapImage();
                bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmi.UriSource = myUri;
                post.Profile = bmi;
            }

            if (jo.ContainsKey("girlsNumber"))
                post.GirlsNumber = (int)jo.GetNamedNumber("girlsNumber");

            if (jo.ContainsKey("guysNumber"))
                post.GuysNumber = (int)jo.GetNamedNumber("guysNumber");

            if (jo.ContainsKey("userName"))
                post.UserName = jo.GetNamedString("userName");

            if (jo.ContainsKey("time"))
                post.Time = jo.GetNamedString("time");

            if (jo.ContainsKey("placeAddresse"))
                post.PlaceAddresse = jo.GetNamedString("placeAddresse");

            if (jo.ContainsKey("popularity"))
                post.Popularity = (int)jo.GetNamedNumber("popularity");

            if (jo.ContainsKey("myPosition"))
                post.MyPosition = jo.GetNamedString("myPosition");

            post.UserId = jo.GetNamedString("userId");

            post.Require = string.Format("+ {0} boys, + {1} girls. {2}", post.GuysNumber, post.GirlsNumber, post.Time);

            return post;
        }
    }
}
