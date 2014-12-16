namespace Tradr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreviousOfferInt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "PreviousOffer", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "PreviousOffer");
        }
    }
}
