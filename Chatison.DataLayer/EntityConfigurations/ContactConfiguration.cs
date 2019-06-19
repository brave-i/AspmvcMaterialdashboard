using Chatison.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Chatison.DataLayer.EntityConfigurations
{
    internal class ContactConfiguration : EntityTypeConfiguration<Contact>
    {
        public ContactConfiguration()
        {
            ToTable("Contacts");

            HasKey(x => x.UserId);

            Property(x => x.UserId).IsRequired().HasMaxLength(128);
            Property(x => x.FirstName).HasMaxLength(255);
            Property(x => x.LastName).HasMaxLength(255);
            Property(x => x.CreatedAt).IsRequired();
            Property(x => x.Source).IsRequired();

            HasMany(x => x.GroupContacts).WithRequired().HasForeignKey(x => x.UserId);
        }
    }
}
