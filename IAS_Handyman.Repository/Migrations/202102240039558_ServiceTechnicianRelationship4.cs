namespace IAS_Handyman.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Text;

    public partial class ServiceTechnicianRelationship4 : DbMigration
    {
        public override void Up()
        {
            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"InitialScripts\ServiceRequests_Technitians_Responsable_Id_.sql");
            DatabaseContext db = new DatabaseContext();
            db.Database.ExecuteSqlCommand(File.ReadAllText(sqlFile, Encoding.GetEncoding(28591)));
        }
        
        public override void Down()
        {
        }
    }
}
