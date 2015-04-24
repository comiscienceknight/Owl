using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;

namespace Owl.Models
{
    public class JsonReceiver
    {
        public async static Task<DataObjects.Post> GetPostByUserId(string userId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", App.OwlbatClient.CurrentUser.MobileServiceAuthenticationToken);
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");
                var user = await httpClient.GetStringAsync(
                    new Uri("http://owlbat.azure-mobile.net/get/getpostbyuserid/" + userId));

                JsonValue jsonValue = JsonValue.Parse(user);
                return AnalyseGetPostByUserId(jsonValue.GetObject());
            }
        }

        private static DataObjects.Post AnalyseGetPostByUserId(IJsonValue userJsonValue)
        {
            JsonObject jo = userJsonValue.GetObject();
            var post = new DataObjects.Post();
            if (jo.ContainsKey("id"))
            {
                post.Id = jo.GetNamedString("id");
                if (jo.ContainsKey("userName"))
                    post.UserName = jo.GetNamedString("userName");
                if (jo.ContainsKey("userId"))
                    post.UserId = jo.GetNamedString("userId");
                if (jo.ContainsKey("profileUrl"))
                    post.ProfileUrl = jo.GetNamedString("profileUrl");
                if (jo.ContainsKey("popularity"))
                    post.UserPopularity = (int)jo.GetNamedNumber("popularity");
                if (jo.ContainsKey("arrivalTime"))
                    post.ArrivalTime = jo.GetNamedString("arrivalTime");
                if (jo.ContainsKey("sexe"))
                    post.Sexe = jo.GetNamedString("sexe");
                if (jo.ContainsKey("girlNumber"))
                    post.GirlsNumber = (int)jo.GetNamedNumber("girlNumber");
                if (jo.ContainsKey("boyNumber"))
                    post.GuysNumber = (int)jo.GetNamedNumber("boyNumber");
                if (jo.ContainsKey("otherInfo"))
                    post.OtherInfo = jo.GetNamedString("otherInfo");
                if (jo.ContainsKey("lookingFor"))
                    post.LookingFor = jo.GetNamedString("lookingFor");
                if (jo.ContainsKey("placeName"))
                    post.Place = jo.GetNamedString("placeName");
                if (jo.ContainsKey("outType"))
                    post.OutType = jo.GetNamedString("outType");
                if (jo.ContainsKey("placeAddresse"))
                    post.PlaceAddresse = jo.GetNamedString("placeAddresse");
                if (jo.ContainsKey("venuePosition"))
                    post.VenuePosition = jo.GetNamedString("venuePosition");
                if (jo.ContainsKey("venueId"))
                    post.VenueId = jo.GetNamedString("venueId");
                if (jo.ContainsKey("venuePopularity"))
                    post.VenuePopularity = (int)jo.GetNamedNumber("venuePopularity");
                if (jo.ContainsKey("venuePhotoUrl1"))
                    post.VenuePhotoUrl1 = jo.GetNamedString("venuePhotoUrl1");
                if (jo.ContainsKey("venuePhotoUrl2"))
                    post.VenuePhotoUrl2 = jo.GetNamedString("venuePhotoUrl2");
                if (jo.ContainsKey("venuePhotoUrl3"))
                    post.VenuePhotoUrl3 = jo.GetNamedString("venuePhotoUrl3");
            }
            return post;
        }
    }
}
