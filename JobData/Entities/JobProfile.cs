using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobData.Entities
{
    public class JobProfile
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [ForeignKey("UserProfile")]
        [Required]
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastestUpdate { get; set; }
        public List<EmployerProfile>? Employers { get; set; }
        public JobProfile()
        {
            Employers = new List<EmployerProfile>();
        }
    }
}
