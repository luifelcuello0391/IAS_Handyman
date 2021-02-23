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
    public class BaseModelData
    {
        [DisplayName("Fecha de creación")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public DateTime CreationDateTime { get; set; } = DateTime.Now;

        [DisplayName("Fecha de modificación")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public DateTime ModificationDateTime { get; set; } = DateTime.Now;

        [DisplayName("Estado del registro")]
        public int RegisterStatus { get; set; } = 1;

        // Translates the register status value to a legible string
        [NotMapped]
        public string RegisterStatusName { 
            get
            {
                return RegisterStatus == 1 ? "Activo" : "Inactivo";
            }
        }
    }
}
