using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwlBatAzureMobileService.Controllers
{
    /// <summary>
    /// http://blogs.msdn.com/b/azuremobile/archive/2014/05/30/realtime-with-signalr-and-azure-mobile-net-backend.aspx
    /// </summary>
    public class ChatHub : Hub
    {
        public ApiServices Services { get; set; }

        [AuthorizeLevel(AuthorizationLevel.User)]
        public string Send(string message)
        {
            return message;
        }
    }
}