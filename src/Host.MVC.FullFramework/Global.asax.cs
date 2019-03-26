using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ContosoTravel.Web.Host.MVC.FullFramework
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Web.Application.Models.SiteModel.SiteTitle = "Contoso Travel - .Net Framework";
            AutofacConfig.RegisterContainer();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
