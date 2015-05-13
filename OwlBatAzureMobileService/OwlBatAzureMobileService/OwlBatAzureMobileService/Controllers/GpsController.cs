using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using OwlBatAzureMobileService.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Specialized;
using Microsoft.AspNet.SignalR;

namespace OwlBatAzureMobileService.Controllers
{
    //http://blogs.msdn.com/b/azuremobile/archive/2014/05/30/realtime-with-signalr-and-azure-mobile-net-backend.aspx

    public class GpsController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/Gps
        public List<JingGegeGp> Get()
        {
            using(var db = new Models.OwlDataClassesDataContext())
            {
                return db.JingGegeGps.ToList();
            }
        }

        [Route("api/updatepos")]
        [HttpPost]
        public async void Post(GpsUnit gpsUnit)
        {
            await Task.Run(() =>
            {
                string userid = gpsUnit.UserId;
                decimal altitude;
                Decimal.TryParse(gpsUnit.Altitude, out altitude);
                decimal longitude;
                Decimal.TryParse(gpsUnit.Longitude, out longitude);
                using (var db = new Models.OwlDataClassesDataContext())
                {
                    if (db.JingGegeGps.Count(p => p.UserId == userid) == 0)
                    {
                        db.JingGegeGps.InsertOnSubmit(new JingGegeGp()
                        {
                            UserId = userid,
                            Altitude = altitude,
                            Longitude = longitude,
                            UpdatedAt = new DateTimeOffset(DateTime.Now)
                        });
                        db.SubmitChanges();
                    }
                    else
                    {
                        var item = db.JingGegeGps.Single(p=> p.UserId == gpsUnit.UserId);
                        item.Longitude = longitude;
                        item.Altitude = altitude;
                        item.UpdatedAt = new DateTimeOffset(DateTime.Now); 
                        db.SubmitChanges();
                    }
                }

                IHubContext hubContext = Services.GetRealtime<GpsHub>();
                hubContext.Clients.All.BoardMyPos(gpsUnit);
            });
        }
    }

    public class GpsUnit
    {
        public string UserId { get; set; }
        public string Altitude { get; set; }
        public string Longitude { get; set; }
    }
}
