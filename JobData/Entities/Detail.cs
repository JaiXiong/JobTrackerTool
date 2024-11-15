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
        public DateTime LatestUpdate { get; set; }
        public string Comments { get; set; } = string.Empty;
        public string Updates { get; set; } = string.Empty;
    }
}
