using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using EventOrganizeService.DataObjects;
using EventOrganizeService.Models;
using Microsoft.WindowsAzure.Mobile.Service;

namespace EventOrganizeService.Controllers
{
    public class OrganizeEventController : TableController<OrganizeEvent>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            EventOrganizeContext context = new EventOrganizeContext();
            DomainManager = new EntityDomainManager<OrganizeEvent>(context, Request, Services);
        }

        // GET tables/OrganizeEvent
        public IQueryable<OrganizeEvent> GetAllTodoItems()
        {
            return Query();
        }

        // GET tables/OrganizeEvent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<OrganizeEvent> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/OrganizeEvent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<OrganizeEvent> PatchTodoItem(string id, Delta<OrganizeEvent> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/OrganizeEvent
        public async Task<IHttpActionResult> PostTodoItem(OrganizeEvent item)
        {
            OrganizeEvent current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/OrganizeEvent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }

    }
}
