namespace Chatison.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactUpdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "IsOptOut", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "IsOptOut");
        }
    }
}
