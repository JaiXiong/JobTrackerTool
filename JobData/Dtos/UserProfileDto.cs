using JobData.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Dtos
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime LatestUpdate { get; set; }
        public ICollection<JobProfile>? JobProfile { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
    }
}
