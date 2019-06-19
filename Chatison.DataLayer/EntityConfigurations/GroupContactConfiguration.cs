using Chatison.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Chatison.DataLayer.EntityConfigurations
{
    internal class GroupContactConfiguration : EntityTypeConfiguration<GroupContact>
    {
        public GroupContactConfiguration()
        {
            ToTable("GroupContacts");

            HasKey(x => x.Id);

            Property(x => x.Id).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.GroupId).IsRequired();
            Property(x => x.UserId).IsRequired().HasMaxLength(128);
            Property(x => x.CreatedAt).IsRequired();

            HasRequired(x => x.Group).WithMany().HasForeignKey(x => x.GroupId);
            HasRequired(x => x.Contact).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
