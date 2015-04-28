using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using OwlBatAzureMobileService.DataObjects;
using System.Text.RegularExpressions;
using OwlBatAzureMobileService.Utils;
using OwlBatAzureMobileService.Models;

namespace OwlBatAzureMobileService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public class CustomRegistrationController : ApiController
    {
        public ApiServices Services { get; set; }

        // POST api/CustomRegistration
        public HttpResponseMessage Post(RegistrationRequest registrationRequest)
        {
            try
            {
                if (!Regex.IsMatch(registrationRequest.username, "^[a-zA-Z0-9]{4,}$"))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid username (at least 4 chars, alphanumeric only)");
                }
                else if (registrationRequest.password.Length < 8)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid password (at least 8 chars required)");
                }

                using (var db = new Models.OwlDataClassesDataContext())
                {
                    if (!db.Accounts.Any(p => p.UserName == registrationRequest.username))
                    {
                        byte[] salt = CustomLoginProviderUtils.GenerateSalt();
                        db.InsertAnAccount(registrationRequest.username, salt, CustomLoginProviderUtils.hash(registrationRequest.password, salt));
                        return this.Request.CreateResponse(HttpStatusCode.Created);
                    }
                    else
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Username already exists");
                }
            }
            catch(Exception exp)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, exp.Message);
            }
        }

    }
}
