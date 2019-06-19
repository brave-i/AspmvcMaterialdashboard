namespace Chatison.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class MobileProvider : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MobileProviders",
                c => new
                {
                    Id = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250, storeType: "nvarchar"),
                    Status = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.MobileProviders");
        }
    }
}
