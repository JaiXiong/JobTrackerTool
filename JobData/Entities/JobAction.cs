﻿using System;
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
        public Guid Id { get; set; }
        public Guid EmployerProfileId { get; set; }
        public DateTime Date { get; set; }
        public DateTime LatestUpdate { get; set; }
        public string? Action { get; set; }
        public string? Method { get; set; } 
        public string? ActionResult { get; set; }
    }
}
