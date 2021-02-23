namespace IAS_Handyman.Repository.Migrations
{
    using IAS_Handyman.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IAS_Handyman.Repository.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IAS_Handyman.Repository.DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            // Validates the existance of the service statuses
            List<ServiceStatus> statuses = context.ServiceStatuses.ToList();
            if (statuses == null || statuses.Count <= 0)
            {
                ServiceStatus assignedStatus = new ServiceStatus();
                assignedStatus.Name = "Asignado";
                assignedStatus.CreationDateTime = DateTime.Now;
                assignedStatus.ModificationDateTime = DateTime.Now;
                assignedStatus.RegisterStatus = 1;

                ServiceStatus executedStatus = new ServiceStatus();
                executedStatus.Name = "Ejecutado";
                executedStatus.CreationDateTime = DateTime.Now;
                executedStatus.ModificationDateTime = DateTime.Now;
                executedStatus.RegisterStatus = 1;

                ServiceStatus cancelledStatus = new ServiceStatus();
                cancelledStatus.Name = "Cancelado";
                cancelledStatus.CreationDateTime = DateTime.Now;
                cancelledStatus.ModificationDateTime = DateTime.Now;
                cancelledStatus.RegisterStatus = 1;

                context.ServiceStatuses.Add(assignedStatus);
                context.ServiceStatuses.Add(executedStatus);
                context.ServiceStatuses.Add(cancelledStatus);

                context.SaveChanges();
            }

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
