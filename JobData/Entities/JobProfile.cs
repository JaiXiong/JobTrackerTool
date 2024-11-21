using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobData.Entities
{
    public class JobProfile
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("UserProfile")]
        [Required]
        public required Guid UserProfileId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LatestUpdate { get; set; }
        [Required]
        public required string ProfileName { get; set; }
        public virtual ICollection<EmployerProfile>? Employers { get; set; }
    }
}
