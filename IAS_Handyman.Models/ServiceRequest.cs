using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAS_Handyman.Models
{
    public class ServiceRequest : BaseModelData
    {
        public ServiceRequest (){}

        // Table auto incremental id
        [Key]
        public int Id { get; set; }

        [DisplayName("Código del servicio")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Code { get; set; }

        [DisplayName("Servicio solicitado")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Description { get; set; }

        [DisplayName("Programado para")]
        public DateTime ScheduleDate { get; set; }

        [DisplayName("¿Es una emergencia?")]
        public bool IsEmergency { get; set; }
        [NotMapped]
        public string IsEmergencyString
        {
            get
            {
                return !IsEmergency ? "Reparación" : "Reparación por emergencia";
            }
        }

        [DisplayName("Estado del servicio")]
        public virtual ServiceStatus CurrentStatus { get; set; }

        public int? Responsable_Id { get; set; }

        [DisplayName("Técnico asignado")]
        public virtual Technician Responsable { get; set; }

        [DisplayName("Fecha y hora de inicio de la atención")]
        public DateTime? StartDateTime { get; set; }

        [DisplayName("Fecha y hora de finalización de la atención")]
        public DateTime? EndDateTime { get; set; }

        [NotMapped]
        public string StartTime { get; set; }

        [NotMapped]
        public string EndTime { get; set; }

        [DisplayName("Horas empleadas")]
        public int Hours { get; set; }

        [NotMapped]
        public int? SelectedTechnicianId { get; set; }
    }
}
