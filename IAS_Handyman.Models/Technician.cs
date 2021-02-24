using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAS_Handyman.Models
{
    public class Technician : BaseModelData
    {
        public Technician() { }
        // Table auto incremental
        [Key]
        public int Id { get; set; }

        [DisplayName("Identificación")]
        [Required(ErrorMessage= "Este campo es requerido")]
        public string Identification { get; set; }

        [DisplayName("Nombre completo")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string FullName { get; set; }

        public virtual List<ServiceRequest> Services { get; set; }
    }
}
