namespace Tradr.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageStatusVariable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "Status");
        }
    }
}
