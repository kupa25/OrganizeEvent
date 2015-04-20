using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using EventOrganizeService.DataObjects;
using EventOrganizeService.Models;

namespace EventOrganizeService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            Database.SetInitializer(new EventOrganizeInitializer());
        }
    }

    public class EventOrganizeInitializer : ClearDatabaseSchemaIfModelChanges<EventOrganizeContext>
    {
        protected override void Seed(EventOrganizeContext context)
        {

            List<OrganizeEvent> organizeEvents = new List<OrganizeEvent>
            {
                new OrganizeEvent {Id = Guid.NewGuid().ToString(), LeaderID = 2},
                new OrganizeEvent {Id = Guid.NewGuid().ToString(), LeaderID = 3},
            };

            foreach (OrganizeEvent organizeEvent in organizeEvents)
            {
                context.Set<OrganizeEvent>().Add(organizeEvent);
            }

            base.Seed(context);
        }
    }
}

