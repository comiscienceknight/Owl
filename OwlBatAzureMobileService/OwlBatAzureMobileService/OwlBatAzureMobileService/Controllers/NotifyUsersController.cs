using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace OwlBatAzureMobileService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class NotifyUsersController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/NotifyUsers
        public string Get()
        {
            Services.Log.Info("Hello from custom controller!");
            return "Hello"; 
        }

        [Route("api/notif")]
        [HttpPost]
        public async Task<bool> Post(JObject data)
        {
            try
            {
                var currentUser = this.User as ServiceUser;
                string wnsToast = 
                    string.Format(
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?><toast><visual><binding template=\"ToastText01\"><text id=\"1\">{0}:{1}</text></binding></visual></toast>",
                    currentUser.Id, data.GetValue("toast").Value<string>());
                WindowsPushMessage message = new WindowsPushMessage();
                message.XmlPayload = wnsToast;
                await Services.Push.SendAsync(message);
                return true;
            }
            catch (Exception e)
            {
                Services.Log.Error(e.ToString());
            }
            return false;
        }
    }
}
