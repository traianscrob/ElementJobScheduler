using Element.Data;
using Element.Data.Interfaces;
using Element.JobScheduler.Configuration;
using Element.JobScheduler.Interfaces;
using Element.JobSchedulerUI.Extensions;
using Element.JobSchedulerUI.JobScheduler;
using LightInject;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Element.JobSchedulerUI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
