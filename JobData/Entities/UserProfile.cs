using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Entities
{
    public class UserProfile
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime LatestUpdate { get; set; }
        public virtual ICollection<JobProfile>? JobProfile { get; set; }
        [Required]
        public required string Name { get; set; }
        public required string? Email { get; set; }
        public required string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
    }
}
