using System.ComponentModel.DataAnnotations;

namespace JobData.Entities
{
    public class JobProfile
    {
        [Required]
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }

        public EmployerProfile Employer { get; set; }

        public JobAction WorkAction { get; set; }
    }
}
