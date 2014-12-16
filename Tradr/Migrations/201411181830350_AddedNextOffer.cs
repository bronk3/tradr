namespace Tradr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNextOffer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "NextOffer", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "NextOffer");
        }
    }
}
