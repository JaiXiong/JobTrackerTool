using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Dtos
{
    public class EmployerProfilePagingDto
    {
        [Key]
        public Guid Id { get; set; }
        public Guid JobProfileId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LatestUpdate { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Phone { get; set; }
    }
}
