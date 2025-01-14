using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Dtos
{
    public class JobActionDto
    {
        public Guid Id { get; set; }
        public Guid EmployerProfileId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LatestUpdate { get; set; }
        public string? Action { get; set; }
        public string? Method { get; set; }
        public string? ActionResult { get; set; }
    }
}
