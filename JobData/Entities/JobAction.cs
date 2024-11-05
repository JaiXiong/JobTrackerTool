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
        public string Action { get; set; }
        public string Method { get; set; }
        public string ActionResult { get; set; }
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
