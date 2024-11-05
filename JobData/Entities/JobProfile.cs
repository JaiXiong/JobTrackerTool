using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JobData.Entities
{
    public class JobProfile
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastestUpdate { get; set; }
        public UserProfile User { get; set; }
        public List<EmployerProfile> Employers { get; set; }
        public JobProfile()
        {
            Employers = new List<EmployerProfile>();
        }
    }
}
