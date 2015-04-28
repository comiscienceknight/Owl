using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using OwlBatAzureMobileService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System.Threading.Tasks;

namespace OwlBatAzureMobileService.Controllers
{
    public class MessageController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/Message
        public string Get()
        {
            Services.Log.Info("Hello from custom controller!");
            return "Hello";
        }

        [Route("message/getrecentsmessages/{userid}")]
        [HttpGet]
        public async Task<GetPostByUserIdResult> GetUserPostByUserId(string userid)
        {
            var currentUser = this.User as ServiceUser;
            string wnsToast =
                string.Format(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?><toast><visual><binding template=\"ToastText01\"><text id=\"1\">{0} Signed in</text></binding></visual></toast>",
                currentUser.Id);
            WindowsPushMessage message = new WindowsPushMessage();
            message.XmlPayload = wnsToast;
            await Services.Push.SendAsync(message, currentUser.Id);

            using (var db = new Models.OwlDataClassesDataContext())
            {
                var results = db.GetPostByUserId(userid).ToList();
                if (results.Count > 0)
                    return results.First();
            }
            return new GetPostByUserIdResult();
        }

    }
}
