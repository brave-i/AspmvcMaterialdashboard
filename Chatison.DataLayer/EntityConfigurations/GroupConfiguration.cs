using Chatison.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Chatison.DataLayer.EntityConfigurations
{
    internal class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            ToTable("Groups");

            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsRequired().HasMaxLength(255);
            Property(x => x.Keyword).HasMaxLength(255);
            Property(x => x.HasKeyword).HasMaxLength(255);
            Property(x => x.CreatedAt).IsRequired();

            HasMany(x => x.GroupContacts).WithRequired().HasForeignKey(x => x.GroupId);
        }
    }
}
