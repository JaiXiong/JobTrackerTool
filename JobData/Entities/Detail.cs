using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Entities
{
    public class Detail
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [ForeignKey("EmployerProfile")]
        [Required]
        public Guid EmployerProfileId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastestUpdate { get; set; }
        public string Comments { get; set; }
        public string Updates { get; set; }
    }
}
