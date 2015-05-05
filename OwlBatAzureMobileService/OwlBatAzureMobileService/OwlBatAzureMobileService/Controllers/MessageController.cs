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
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Web;

namespace OwlBatAzureMobileService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
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

        [Route("message/getmessages/{userid}/{createdtimeticks}")]
        [HttpGet]
        public List<GetMessagesResult> GetMessages(string userid, string createdtimeticks)
        {
            DateTimeOffset createdTime = new DateTimeOffset(new DateTime(Convert.ToInt64(createdtimeticks)));
            using (var db = new Models.OwlDataClassesDataContext())
            {
                return db.GetMessages(userid, createdTime, 20).ToList();
            }
        }

        [Route("message/sendmsg")]
        [HttpPost]
        public async Task<bool> SendMessage()//JObject data)
        {
            try
            {
                HttpRequestMessage request = this.Request;
                string querystring = await request.Content.ReadAsStringAsync();
                NameValueCollection qscoll = HttpUtility.ParseQueryString(querystring);

                using (var db = new Models.OwlDataClassesDataContext())
                {
                    db.InsertAnMessage(qscoll["message"], qscoll["fromuserid"], qscoll["touserid"]);
                }
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
