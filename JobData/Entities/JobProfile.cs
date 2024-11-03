using System.ComponentModel.DataAnnotations;

namespace JobEntities.Entities
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
