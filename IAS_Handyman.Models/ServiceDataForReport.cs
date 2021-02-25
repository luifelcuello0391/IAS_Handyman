using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAS_Handyman.Models
{
    public class ServiceDataForReport
    {
        public ServiceDataForReport() { }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime ScheduleDate { get; set; }
        public bool IsEmergency { get; set; }
        public ServiceStatus CurrentStatus { get; set; }
        public Technician Responsable { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int Hours { get; set; }
    }
}
