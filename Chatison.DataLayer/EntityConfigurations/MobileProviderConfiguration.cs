using Chatison.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Chatison.DataLayer.EntityConfigurations
{
    internal class MobileProviderConfiguration : EntityTypeConfiguration<MobileProvider>
    {
        public MobileProviderConfiguration()
        {
            ToTable("MobileProviders");

            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).IsRequired().HasMaxLength(250);
            Property(x => x.Status).IsRequired();
        }
    }
}
