//using System.Configuration;
//using System.Data.Entity;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using MySql.Data.Entity;

//namespace Chatison.Models
//{
//    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

//    [DbConfigurationType(typeof(MySqlEFConfiguration))]
//    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
//    {
//        public ApplicationDbContext()
//           : base("DefaultConnection", throwIfV1Schema: false)
//        {
//            var _ = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
//            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
//        }

//        public static ApplicationDbContext Create()
//        {
//            return new ApplicationDbContext();
//        }

//        public DbSet<Contact> Contacts { get; set; }

//        public DbSet<Organization> Organizations { get; set; }
//    }
//}