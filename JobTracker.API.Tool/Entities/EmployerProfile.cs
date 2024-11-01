﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEntities.Entities
{
    public class EmployerProfile
    {
        [Required]
        public Guid Id { get; set; }
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

    }
}
