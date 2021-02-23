namespace IAS_Handyman.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceExecutionDatesNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceRequests", "StartDateTime", c => c.DateTime());
            AlterColumn("dbo.ServiceRequests", "EndDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceRequests", "EndDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ServiceRequests", "StartDateTime", c => c.DateTime(nullable: false));
        }
    }
}
