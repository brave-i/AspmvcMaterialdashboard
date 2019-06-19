namespace Chatison.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityUpdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CurrentSignInAt", c => c.DateTime(precision: 0));
            AddColumn("dbo.AspNetUsers", "CurrentSignInIp", c => c.String(maxLength: 40, storeType: "nvarchar"));
            AddColumn("dbo.AspNetUsers", "LastSignInAt", c => c.DateTime(precision: 0));
            AddColumn("dbo.AspNetUsers", "LastSignInIp", c => c.String(maxLength: 40, storeType: "nvarchar"));
            AddColumn("dbo.AspNetUsers", "SignInCount", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "LastLogOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LastLogOn", c => c.DateTime(precision: 0));
            DropColumn("dbo.AspNetUsers", "SignInCount");
            DropColumn("dbo.AspNetUsers", "LastSignInIp");
            DropColumn("dbo.AspNetUsers", "LastSignInAt");
            DropColumn("dbo.AspNetUsers", "CurrentSignInIp");
            DropColumn("dbo.AspNetUsers", "CurrentSignInAt");
        }
    }
}
