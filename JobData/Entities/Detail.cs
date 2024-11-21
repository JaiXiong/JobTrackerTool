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
        public Guid Id { get; set; }
        [ForeignKey("EmployerProfile")]
        [Required]
        public required Guid EmployerProfileId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LatestUpdate { get; set; }
        [Column(TypeName = "varchar(500)")]
        [MaxLength(500)]
        public string? Comments { get; set; }
        [Column(TypeName = "varchar(500)")]
        [MaxLength(500)]
        public string? Updates { get; set; }
    }
}
