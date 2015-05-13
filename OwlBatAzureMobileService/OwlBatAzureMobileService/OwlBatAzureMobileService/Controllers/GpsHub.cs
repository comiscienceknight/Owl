using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwlBatAzureMobileService.Controllers
{
    public class GpsHub : Hub
    {
        public ApiServices Services { get; set; }

        [AuthorizeLevel(AuthorizationLevel.User)]
        public string Send(string message)
        {
            return message;
        }

        public GpsUnit BoardMyPos(GpsUnit gpsUnit)
        {
            return gpsUnit;
        }
    }
}