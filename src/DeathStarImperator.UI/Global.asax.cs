using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DeathStarImperator.Core;
using StructureMap;

namespace DeathStarImperator.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private Imperator _imperator;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _imperator = ObjectFactory.GetInstance<Imperator>();
            _imperator.InitializeConfig();
            _imperator.StartImperator();
        }
    }
}
