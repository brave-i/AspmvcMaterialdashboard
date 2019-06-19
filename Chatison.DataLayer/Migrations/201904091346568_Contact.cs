namespace Chatison.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Contact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        FirstName = c.String(maxLength: 255, storeType: "nvarchar"),
                        LastName = c.String(maxLength: 255, storeType: "nvarchar"),
                        ReceiveSms = c.Boolean(nullable: false),
                        ReceivedFirstSms = c.Boolean(nullable: false),
                        OrganizationId = c.Int(),
                        MobileProviderId = c.Int(),
                        BirthDate = c.DateTime(precision: 0),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                        OptInSms = c.Boolean(nullable: false),
                        OptInEmail = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.GroupContacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        Keyword = c.String(maxLength: 255, storeType: "nvarchar"),
                        HasKeyword = c.String(maxLength: 255, storeType: "nvarchar"),
                        OraganizationId = c.Int(),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupContacts", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupContacts", "UserId", "dbo.Contacts");
            DropIndex("dbo.GroupContacts", new[] { "UserId" });
            DropIndex("dbo.GroupContacts", new[] { "GroupId" });
            DropTable("dbo.Groups");
            DropTable("dbo.GroupContacts");
            DropTable("dbo.Contacts");
        }
    }
}
