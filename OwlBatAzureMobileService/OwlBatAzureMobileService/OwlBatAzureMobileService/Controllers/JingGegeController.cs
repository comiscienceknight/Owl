using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace OwlBatAzureMobileService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class JingGegeController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/JingGege
        public string Get()
        {
            Services.Log.Info("Hello from custom controller!");
            return "Hello";
        }

        [Route("api/diner")]
        [HttpGet]
        public DinerInfo Diner()
        {
            return new DinerInfo()
            {
                DinerPlace = "Not defined yet",
                DinerTime = new DateTimeOffset(new DateTime(2015, 5, 12, 20, 0, 0))
            };
        }
    }

    public class DinerInfo
    {
        public DateTimeOffset DinerTime { get; set; }
        public string DinerPlace { get; set; }
    }
}
