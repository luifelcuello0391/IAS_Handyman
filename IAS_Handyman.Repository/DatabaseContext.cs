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
        }

        //public DatabaseContext(DbContextOptions<DatabaseContext> options)
        //    : base(options)
        //{
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseLazyLoadingProxies();
        //}

        public DbSet<Technician> Technicians { get; set; }
        public DbSet<ServiceRequest> Services { get; set; }
        public DbSet<ServiceStatus> ServiceStatuses { get; set; }
    }
}
