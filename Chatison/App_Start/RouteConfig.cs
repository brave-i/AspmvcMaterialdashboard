using System.Web.Mvc;
using System.Web.Routing;

namespace Chatison
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "CChatison.Controllers", "Chatison.Areas.Admin.Controllers" }
            );
        }
    }
}
