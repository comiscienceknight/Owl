using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Owl.DataObjects;
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

namespace Owl.ViewModel
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
            Messenger.Default.Register<Message.RefreshPostsMessage>(this, msg =>
            {
                RefreshPost();
            });
        }

        private async void LoadPosts()
        {
            Messenger.Default.Send<LoadingAnimationMessage>(new LoadingAnimationMessage(true));
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                var posts = await httpClient.GetStringAsync(
                    new Uri("http://owlbat.azure-mobile.net/get/getposts/2015-03-10/010101"));

                JsonValue jsonValue = JsonValue.Parse(posts);
                AnalysePostJsonValueArray(jsonValue);
            }
            Messenger.Default.Send<LoadingAnimationMessage>(new LoadingAnimationMessage());
        }

        public void RefreshPost()
        {
            PostCollection.Clear();
            LoadPosts();
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

            post.Place = jo.GetNamedString("venueName");

            if (jo.ContainsKey("description"))
                post.OtherInfo = jo.GetNamedString("description");

            if (jo.ContainsKey("userProfileUrl1"))
            {
                post.ProfileUrl = jo.GetNamedString("userProfileUrl1");
                if (!string.IsNullOrWhiteSpace(post.ProfileUrl))
                {
                    Uri myUri = new Uri(post.ProfileUrl + "?Width=175", UriKind.Absolute);
                    BitmapImage bmi = new BitmapImage();
                    bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmi.UriSource = myUri;
                    post.Profile = bmi;
                }
            }
            if (jo.ContainsKey("userProfileUrl2"))
            {
                post.ProfileUrl2 = jo.GetNamedString("userProfileUrl2");
            }
            if (jo.ContainsKey("userProfileUrl3"))
            {
                post.ProfileUrl2 = jo.GetNamedString("userProfileUrl3");
            }

            if (jo.ContainsKey("placeAddresse"))
            {
                post.PlaceAddresse = jo.GetNamedString("placeAddresse");
            }

            if (jo.ContainsKey("venuePosition"))
            {
                post.VenuePosition = jo.GetNamedString("venuePosition");
            }

            if (jo.ContainsKey("venuePopularity"))
            {
                post.VenuePopularity = (int)jo.GetNamedNumber("venuePopularity");
            }

            if (jo.ContainsKey("venuePhotoUrl1"))
            {
                post.VenuePhotoUrl1 = jo.GetNamedString("venuePhotoUrl1");
            }
            if (jo.ContainsKey("venuePhotoUrl2"))
            {
                post.VenuePhotoUrl2 = jo.GetNamedString("venuePhotoUrl2");
            }
            if (jo.ContainsKey("venuePhotoUrl3"))
            {
                post.VenuePhotoUrl3 = jo.GetNamedString("venuePhotoUrl3");
            }

            if (jo.ContainsKey("girlNumber"))
                post.GirlsNumber = (int)jo.GetNamedNumber("girlNumber");

            if (jo.ContainsKey("boyNumber"))
                post.GuysNumber = (int)jo.GetNamedNumber("boyNumber");

            if (jo.ContainsKey("userName"))
                post.UserName = jo.GetNamedString("userName");

            if (jo.ContainsKey("time"))
                post.Time = jo.GetNamedString("time");

            if (jo.ContainsKey("userPopularity"))
                post.UserPopularity = (int)jo.GetNamedNumber("userPopularity");

            post.UserId = jo.GetNamedString("userId");

            post.Require = string.Format("+ {0} boys, + {1} girls. {2}", post.GuysNumber, post.GirlsNumber, post.Time);

            return post;
        }
    }
}
