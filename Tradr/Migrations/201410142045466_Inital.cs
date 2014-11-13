namespace Tradr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        DateTimeAdded = c.DateTime(nullable: false),
                        Views = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        OfferId = c.Int(nullable: false, identity: true),
                        DateTimeInitial = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        SenderId = c.Int(nullable: false),
                        RecieverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OfferId)
                .ForeignKey("dbo.Users", t => t.RecieverId)
                .ForeignKey("dbo.Users", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.RecieverId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        DateTimeMessage = c.DateTime(nullable: false),
                        MessageText = c.String(),
                        Offer_OfferId = c.Int(),
                        Reciever_UserId = c.Int(),
                        Sender_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Offers", t => t.Offer_OfferId)
                .ForeignKey("dbo.Users", t => t.Reciever_UserId)
                .ForeignKey("dbo.Users", t => t.Sender_UserId)
                .Index(t => t.Offer_OfferId)
                .Index(t => t.Reciever_UserId)
                .Index(t => t.Sender_UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailAddress = c.String(),
                        Password = c.String(),
                        DateTimeRegistered = c.DateTime(nullable: false),
                        SecondaryEmail = c.String(),
                        Phone = c.String(),
                        Street = c.String(),
                        City = c.String(),
                        AreaCode = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.TagUsers",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.User_UserId })
                .ForeignKey("dbo.Tags", t => t.Tag_TagId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Tag_TagId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.DesiredItemOffer",
                c => new
                    {
                        ItemId = c.Int(nullable: false),
                        OfferId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItemId, t.OfferId })
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.OfferId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.OfferId);
            
            CreateTable(
                "dbo.ProposedItemOffer",
                c => new
                    {
                        ItemId = c.Int(nullable: false),
                        OfferId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItemId, t.OfferId })
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.OfferId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.OfferId);
            
            CreateTable(
                "dbo.ItemTagsTags",
                c => new
                    {
                        ItemId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItemId, t.TagId })
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.ItemWantsTags",
                c => new
                    {
                        ItemId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItemId, t.TagId })
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.TagId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemWantsTags", "TagId", "dbo.Tags");
            DropForeignKey("dbo.ItemWantsTags", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "UserId", "dbo.Users");
            DropForeignKey("dbo.ItemTagsTags", "TagId", "dbo.Tags");
            DropForeignKey("dbo.ItemTagsTags", "ItemId", "dbo.Items");
            DropForeignKey("dbo.ProposedItemOffer", "OfferId", "dbo.Offers");
            DropForeignKey("dbo.ProposedItemOffer", "ItemId", "dbo.Items");
            DropForeignKey("dbo.DesiredItemOffer", "OfferId", "dbo.Offers");
            DropForeignKey("dbo.DesiredItemOffer", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Offers", "SenderId", "dbo.Users");
            DropForeignKey("dbo.Offers", "RecieverId", "dbo.Users");
            DropForeignKey("dbo.Messages", "Sender_UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "Reciever_UserId", "dbo.Users");
            DropForeignKey("dbo.TagUsers", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.TagUsers", "Tag_TagId", "dbo.Tags");
            DropForeignKey("dbo.Messages", "Offer_OfferId", "dbo.Offers");
            DropIndex("dbo.ItemWantsTags", new[] { "TagId" });
            DropIndex("dbo.ItemWantsTags", new[] { "ItemId" });
            DropIndex("dbo.ItemTagsTags", new[] { "TagId" });
            DropIndex("dbo.ItemTagsTags", new[] { "ItemId" });
            DropIndex("dbo.ProposedItemOffer", new[] { "OfferId" });
            DropIndex("dbo.ProposedItemOffer", new[] { "ItemId" });
            DropIndex("dbo.DesiredItemOffer", new[] { "OfferId" });
            DropIndex("dbo.DesiredItemOffer", new[] { "ItemId" });
            DropIndex("dbo.TagUsers", new[] { "User_UserId" });
            DropIndex("dbo.TagUsers", new[] { "Tag_TagId" });
            DropIndex("dbo.Messages", new[] { "Sender_UserId" });
            DropIndex("dbo.Messages", new[] { "Reciever_UserId" });
            DropIndex("dbo.Messages", new[] { "Offer_OfferId" });
            DropIndex("dbo.Offers", new[] { "RecieverId" });
            DropIndex("dbo.Offers", new[] { "SenderId" });
            DropIndex("dbo.Items", new[] { "UserId" });
            DropTable("dbo.ItemWantsTags");
            DropTable("dbo.ItemTagsTags");
            DropTable("dbo.ProposedItemOffer");
            DropTable("dbo.DesiredItemOffer");
            DropTable("dbo.TagUsers");
            DropTable("dbo.Tags");
            DropTable("dbo.Users");
            DropTable("dbo.Messages");
            DropTable("dbo.Offers");
            DropTable("dbo.Items");
        }
    }
}
