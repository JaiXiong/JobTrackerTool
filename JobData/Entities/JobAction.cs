using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Entities
{
    public class JobAction
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [ForeignKey("EmployerProfile")]
        [Required]
        public Guid EmployerProfileId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LatestUpdate { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string ActionResult { get; set; } = string.Empty;
        public enum ActionType
        {
            Apply,
            Interview,
            Offer,
            Reject
        }
        public enum ContactMethod
        {
            Email,
            Phone,
            InPerson,
            Other
        }
        public enum Result
        {
            Success,
            Failure
        }
    }
}
