using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwlBatAzureMobileService.DataObjects
{
    public class Post : EntityData
    {
        public string Id { get; set; }
        public string PlaceName { get; set; }
        public string UserId { get; set; }
        public string Time { get; set; }
        public int GuysNumber { get; set; }
        public int GirlsNumber { get; set; }
        public string Description { get; set; }
        public string ProfileUrl { get; set; }
        public string ProfileUrl2 { get; set; }
        public string ProfileUrl3 { get; set; }
        public string ProfileUrl4 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}