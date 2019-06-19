using Chatison.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Chatison.DataLayer.EntityConfigurations
{
    public class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public CountryConfiguration()
        {
            ToTable("Countries");

            HasKey(x => x.Id)
                .Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.IsoCode).HasMaxLength(5);
            Property(x => x.Name).IsRequired().HasMaxLength(250);
            Property(x => x.IsActive).IsRequired();
        }
    }
}
