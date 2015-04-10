using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwlBatAzureMobileService.DataObjects
{
    public class Post : EntityData
    {
        public string VenueName { get; set; }
        public string PostId { get; set; }
        public string UserId { get; set; }
        public string Time { get; set; }
        public int? GuysNumber { get; set; }
        public int? GirlsNumber { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string ProfileUrl { get; set; }
        public string Sexe { get; set; }
        public string PlaceAddresse { get; set; }
        public string MyPosition { get; set; }
        public int? Popularity { get; set; }
        public string VenueId { get; set; }
    }
}