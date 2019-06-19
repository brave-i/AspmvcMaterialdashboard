using Chatison.DataLayer;
using Chatison.Infrastructure.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Chatison
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //seed roles
            var dbContext = DependencyResolver.Current.GetService<DataContext>();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));
            //seed admin role
            if (!roleManager.RoleExists(Utilities.Constants.UserRoles.Admin))
            {
                var role = new IdentityRole { Name = Utilities.Constants.UserRoles.Admin };
                roleManager.Create(role);
            }
            //seed user role
            if (!roleManager.RoleExists(Utilities.Constants.UserRoles.User))
            {
                var role = new IdentityRole { Name = Utilities.Constants.UserRoles.User };
                roleManager.Create(role);
            }
            //run data seeder
            var dataSeedManager = DependencyResolver.Current.GetService<IDataSeedManager>();

            dataSeedManager.SeedMobileProvidersAsync().Wait();
        }
    }
}
