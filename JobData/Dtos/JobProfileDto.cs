using JobData.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Dtos
{
    public class JobProfileDto
    {
        public Guid Id { get; set; }
        public Guid UserProfileId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LatestUpdate { get; set; }
        public string ProfileName { get; set; }
    }
}
