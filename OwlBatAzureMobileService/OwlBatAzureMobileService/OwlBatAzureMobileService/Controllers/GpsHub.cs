using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwlBatAzureMobileService.Controllers
{
    [System.Web.Http.AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class GpsHub : Hub
    {
        public ApiServices Services { get; set; }

        [System.Web.Http.AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public string Send(string message)
        {
            return message;
        }
    }
}