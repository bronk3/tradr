namespace Tradr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        ItemImage = c.String(),
                        Item_ItemId = c.Int(),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.Items", t => t.Item_ItemId)
                .Index(t => t.Item_ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "Item_ItemId", "dbo.Items");
            DropIndex("dbo.Images", new[] { "Item_ItemId" });
            DropTable("dbo.Images");
        }
    }
}
