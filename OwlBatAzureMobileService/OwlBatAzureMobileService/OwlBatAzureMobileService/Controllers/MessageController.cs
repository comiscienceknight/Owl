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

        [Route("message/createdpair/{userid1}/{userid2}")]
        [HttpGet]
        public string CreateMessageUserPair(string userid1, string userid2)
        {
            try
            {
                using(var db = new Models.OwlDataClassesDataContext())
                {
                    if(db.MessageUserPairs.Any(p=>(p.User1Id == userid1 && p.User2Id == userid2) || 
                        (p.User1Id == userid2 && p.User2Id == userid1)))
                    {
                        return db.MessageUserPairs.Where(p => (p.User1Id == userid1 && p.User2Id == userid2) ||
                        (p.User1Id == userid2 && p.User2Id == userid1)).First().Id;
                    }
                    else if(!string.IsNullOrWhiteSpace(userid2) && !string.IsNullOrWhiteSpace(userid1))
                    {
                        string newPairId = Guid.NewGuid().ToString();
                        db.MessageUserPairs.InsertOnSubmit(new MessageUserPair()
                        {
                            User1Id = userid1,
                            User2Id = userid2,
                            Id = newPairId
                        });
                        db.SubmitChanges();
                        return newPairId;
                    }
                }
            }
            catch (Exception e)
            {
                Services.Log.Error(e.ToString());
            }
            return "";
        }

        [Route("message/sendmessage")]
        [HttpPost]
        public bool SendMessage(JObject data)
        {
            try
            {
                string pairId = data.GetValue("pairid").Value<string>();
                string message = data.GetValue("message").Value<string>();
                using(var db = new Models.OwlDataClassesDataContext())
                {
                    db.InsertAnMessage(message, pairId);
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
