namespace Chatison.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactUpdate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Source", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Source");
        }
    }
}
