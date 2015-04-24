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
            PostCollection.Clear();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                var posts = await httpClient.GetStringAsync(
                    new Uri("http://owlbat.azure-mobile.net/get/getposts"));

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

            if (jo.ContainsKey("userId"))
                post.UserId = jo.GetNamedString("userId");
            if (jo.ContainsKey("userName"))
                post.UserName = jo.GetNamedString("userName");
            if (jo.ContainsKey("profileUrl"))
            {
                post.ProfileUrl = jo.GetNamedString("profileUrl");
                if (!string.IsNullOrWhiteSpace(post.ProfileUrl))
                {
                    Uri myUri = new Uri(post.ProfileUrl + "?Width=175", UriKind.Absolute);
                    BitmapImage bmi = new BitmapImage();
                    bmi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmi.UriSource = myUri;
                    post.Profile = bmi;
                }
            }
            if (jo.ContainsKey("userPopularity"))
                post.UserPopularity = (int)jo.GetNamedNumber("userPopularity");
            if (jo.ContainsKey("venueId"))
                post.VenueId = jo.GetNamedString("venueId");
            if (jo.ContainsKey("placeName"))
                post.Place = jo.GetNamedString("placeName");
            if (jo.ContainsKey("arrivalTime"))
                post.ArrivalTime = jo.GetNamedString("arrivalTime");
            if (jo.ContainsKey("sexe"))
                post.Sexe = jo.GetNamedString("sexe");
            if (jo.ContainsKey("girlNumber"))
                post.GirlsNumber = (int)jo.GetNamedNumber("girlNumber");
            if (jo.ContainsKey("boyNumber"))
                post.GuysNumber = (int)jo.GetNamedNumber("boyNumber");
            if (jo.ContainsKey("birthday"))
                post.Birthday = jo.GetNamedString("birthday");
            if (jo.ContainsKey("otherInfo"))
                post.OtherInfo = jo.GetNamedString("otherInfo");
            if (jo.ContainsKey("codeDress"))
                post.DressCode = jo.GetNamedString("codeDress");
            if (jo.ContainsKey("outType"))
                post.OutType = jo.GetNamedString("outType");
            if (jo.ContainsKey("lookingFor"))
                post.LookingFor = jo.GetNamedString("lookingFor");
            if (jo.ContainsKey("placeAddresse"))
                post.PlaceAddresse = jo.GetNamedString("placeAddresse");
            if (jo.ContainsKey("venuePosition"))
                post.VenuePosition = jo.GetNamedString("venuePosition");
            if (jo.ContainsKey("venuePopularity"))
                post.VenuePopularity = (int)jo.GetNamedNumber("venuePopularity");
            if (jo.ContainsKey("venuePhotoUrl1"))
                post.VenuePhotoUrl1 = jo.GetNamedString("venuePhotoUrl1");
            if (jo.ContainsKey("venuePhotoUrl2"))
                post.VenuePhotoUrl2 = jo.GetNamedString("venuePhotoUrl2");
            if (jo.ContainsKey("venuePhotoUrl3"))
                post.VenuePhotoUrl3 = jo.GetNamedString("venuePhotoUrl3");

            post.Require = string.Format("+{0} boys, +{1} girls. {2}", post.GuysNumber, post.GirlsNumber, post.ArrivalTime);

            return post;
        }
    }
}
