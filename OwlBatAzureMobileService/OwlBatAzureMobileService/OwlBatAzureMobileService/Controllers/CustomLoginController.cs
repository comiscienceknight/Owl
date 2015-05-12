using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using OwlBatAzureMobileService.DataObjects;
using System.Security.Claims;
using OwlBatAzureMobileService.Utils;
using System.Data.Linq;
using System.Net.Http;
using System.Text;

namespace OwlBatAzureMobileService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class CustomLoginController : ApiController
    {
        public ApiServices Services { get; set; }
        public IServiceTokenHandler handler { get; set; }

        // GET api/CustomLogin
        public string Get()
        {
            Services.Log.Info("Hello from custom controller!");
            return "Hello";
        }

        // POST api/CustomLogin
        [Route("api/customlogin")]
        [HttpPost]
        public HttpResponseMessage Post(LoginRequest loginRequest)
        {
            using (var db = new Models.OwlDataClassesDataContext())
            {
                if (db.Accounts.Any(p => p.UserName == loginRequest.username))
                {
                    var account = db.Accounts.Where(p => p.UserName == loginRequest.username).First();
                    byte[] incoming = CustomLoginProviderUtils.hash(loginRequest.password, ConvertBinaryToByte(account.Salt));

                    if (CustomLoginProviderUtils.SlowEquals(incoming, ConvertBinaryToByte(account.SaltedAndHashedPassword)))
                    {
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity();
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginRequest.username));
                        LoginResult loginResult = new CustomLoginProvider(handler).CreateLoginResult(claimsIdentity, Services.Settings.MasterKey);
                        return this.Request.CreateResponse(HttpStatusCode.OK, loginResult);
                    }
                }
            }
            return this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid username or password");
        }

        public byte[] ConvertBinaryToByte(Binary binary)
        {
            return binary.ToArray();
        }

    }
}
