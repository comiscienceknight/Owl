using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlWindowsPhoneApp.DataObjects
{
    public class Place 
    {
        public string Id { get; set; }
        [JsonProperty(PropertyName = "placeName")]
        public string PlaceName { get; set; }
        [JsonProperty(PropertyName = "placeAddresse")]
        public string PlaceAddresse { get; set; }
        [JsonProperty(PropertyName = "myPosition")]
        public string MyPosition { get; set; }
        [JsonProperty(PropertyName = "popularity")]
        public int Popularity { get; set; }
    }
}
