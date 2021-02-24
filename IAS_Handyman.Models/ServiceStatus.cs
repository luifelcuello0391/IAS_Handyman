using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAS_Handyman.Models
{
    public class ServiceStatus : BaseModelData
    {
        public ServiceStatus() { }

        // Table auto incremental id
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
