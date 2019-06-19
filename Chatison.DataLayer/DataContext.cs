using Chatison.DataLayer.EntityConfigurations;
using Chatison.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System.Data.Entity;

namespace Chatison.DataLayer
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public IDbSet<Country> Countries { get; set; }
        public IDbSet<MobileProvider> MobileProviders { get; set; }
        public IDbSet<Group> Groups { get; set; }
        public IDbSet<Contact> Contacts { get; set; }
        public IDbSet<GroupContact> GroupContacts { get; set; }

        public DataContext()
            : base("DataConnection", throwIfV1Schema: false)
        {
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Configurations.Add(new CountryConfiguration());
            builder.Configurations.Add(new MobileProviderConfiguration());
            builder.Configurations.Add(new GroupConfiguration());
            builder.Configurations.Add(new ContactConfiguration());
            builder.Configurations.Add(new GroupContactConfiguration());
        }
    }
}
