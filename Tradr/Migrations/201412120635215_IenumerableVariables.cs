namespace Tradr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IenumerableVariables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "DateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Messages", "DateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Offers", "DateTimeInitial");
            DropColumn("dbo.Messages", "DateTimeMessage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "DateTimeMessage", c => c.DateTime(nullable: false));
            AddColumn("dbo.Offers", "DateTimeInitial", c => c.DateTime(nullable: false));
            DropColumn("dbo.Messages", "DateTime");
            DropColumn("dbo.Offers", "DateTime");
        }
    }
}
