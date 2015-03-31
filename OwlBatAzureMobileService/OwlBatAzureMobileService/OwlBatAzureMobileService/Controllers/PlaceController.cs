using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using OwlBatAzureMobileService.DataObjects;
using OwlBatAzureMobileService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace OwlBatAzureMobileService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class PlaceController : TableController<Place>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Place>(context, Request, Services);
        }

        // GET tables/Place
        public IQueryable<Place> GetAllPlace()
        {
            return Query(); 
        }

        // GET tables/Place/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Place> GetPlace(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Place/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Place> PatchPlace(string id, Delta<Place> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Place
        public async Task<IHttpActionResult> PostPlace(Place item)
        {
            Place current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Place/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePlace(string id)
        {
             return DeleteAsync(id);
        }

    }
}