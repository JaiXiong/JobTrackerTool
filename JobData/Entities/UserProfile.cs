﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Entities
{
    public class UserProfile
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [ForeignKey("JobProfile")]
        public Guid JobProfileId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastestUpdate { get; set; }
       
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}