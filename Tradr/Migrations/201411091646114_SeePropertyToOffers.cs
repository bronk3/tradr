namespace Tradr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeePropertyToOffers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "See", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "See");
        }
    }
}
