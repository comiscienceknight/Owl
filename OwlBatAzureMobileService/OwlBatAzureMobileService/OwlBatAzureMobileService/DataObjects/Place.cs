using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwlBatAzureMobileService.DataObjects
{
    public class Place : EntityData
    {
        public string PlaceName { get; set; }
        public string PlaceAddresse { get; set; }
        public string MyPosition { get; set; }
        public int Popularity { get; set; }
    }
}