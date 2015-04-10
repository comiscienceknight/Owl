using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwlBatAzureMobileService.DataObjects
{
    public class VenueBasicInfo : EntityData
    {
        public string PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string PlaceAddresse { get; set; }
    }

    public class Venue : EntityData
    {
        public string PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string PlaceAddresse { get; set; }
        public int? Popularity { get; set; }
        public string MyPosition { get; set; }
        public string VenuePhotoUrl1 { get; set; }
        public string VenuePhotoUrl2 { get; set; }
        public string VenuePhotoUrl3 { get; set; }
    }

    public class VenuePhoto : EntityData
    {
        public string Id { get; set; }
        public string PlaceId { get; set; }
        public string VenuePhotoUrl1 { get; set; }
        public string VenuePhotoUrl2 { get; set; }
        public string VenuePhotoUrl3 { get; set; }
    }
}