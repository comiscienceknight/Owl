using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace OwlBatAzureMobileService.Controllers
{
    public class NotifyUsersController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/NotifyUsers
        public string Get()
        {
            Services.Log.Info("Hello from custom controller!");
            return "Hello"; 
        }

        public async Task Post(JObject data)
        { 
            string wnsToast = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><toast><visual><binding template=\"ToastText01\"><text id=\"1\">{0}</text></binding></visual></toast>", 
                data.GetValue("toast").ToString());
            WindowsPushMessage message = new WindowsPushMessage();
            message.XmlPayload = wnsToast;
            await Services.Push.SendAsync(message);
        }
    }
}
