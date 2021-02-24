using IAS_Handyman.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAS_Handyman.Repository
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("IASHandymanContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceRequest>()
                .HasOptional(r => r.Responsable)
                .WithMany(r => r.Services)
                .HasForeignKey(r => r.Responsable_Id)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Technician> Technicians { get; set; }
        public DbSet<ServiceRequest> Services { get; set; }
        public DbSet<ServiceStatus> ServiceStatuses { get; set; }
    }
}
