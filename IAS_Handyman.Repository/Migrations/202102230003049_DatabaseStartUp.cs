namespace IAS_Handyman.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Text;

    public partial class DatabaseStartUp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ScheduleDate = c.DateTime(nullable: false),
                        IsEmergency = c.Boolean(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        Hours = c.Int(nullable: false),
                        CreationDateTime = c.DateTime(nullable: false),
                        ModificationDateTime = c.DateTime(nullable: false),
                        RegisterStatus = c.Int(nullable: false),
                        CurrentStatus_Id = c.Int(),
                        Responsable_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceStatus", t => t.CurrentStatus_Id)
                .ForeignKey("dbo.Technicians", t => t.Responsable_Id)
                .Index(t => t.CurrentStatus_Id)
                .Index(t => t.Responsable_Id);
            
            CreateTable(
                "dbo.ServiceStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreationDateTime = c.DateTime(nullable: false),
                        ModificationDateTime = c.DateTime(nullable: false),
                        RegisterStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Technicians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identification = c.String(nullable: false),
                        FullName = c.String(nullable: false),
                        CreationDateTime = c.DateTime(nullable: false),
                        ModificationDateTime = c.DateTime(nullable: false),
                        RegisterStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceRequests", "Responsable_Id", "dbo.Technicians");
            DropForeignKey("dbo.ServiceRequests", "CurrentStatus_Id", "dbo.ServiceStatus");
            DropIndex("dbo.ServiceRequests", new[] { "Responsable_Id" });
            DropIndex("dbo.ServiceRequests", new[] { "CurrentStatus_Id" });
            DropTable("dbo.Technicians");
            DropTable("dbo.ServiceStatus");
            DropTable("dbo.ServiceRequests");
        }
    }
}
