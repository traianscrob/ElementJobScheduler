using Element.JobSchedulerUI.JobScheduler;
using LightInject;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Element.JobSchedulerUI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = GetServiceContainer();

            GlobalConfiguration.Configuration.UseJobScheduler((configuration) => {
                configuration.OnErrorCallback = (job, ex) => { };
                configuration.OnJobStart = (job) =>
                {
                };
                configuration.OnJobEnd = (job) =>
                {
                };
            });
        }

        private static IServiceContainer GetServiceContainer()
        {
            var container = new ServiceContainer();
            container.RegisterControllers();
            container.EnableMvc();

            return container;
        }
    }
}
