using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Entities
{
    public class EmployerProfile
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [ForeignKey("JobProfile")]
        [Required]
        public Guid JobProfileId { get; set; }
        public string Name { get; set; }
        public string title { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public JobAction Result { get; set; }
        public Detail Detail { get; set; }

        public EmployerProfile()
        {
            Detail = new Detail();
            Result = new JobAction();
        }
    }
}
