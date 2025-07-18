using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Entities
{
    public class EmailConfirmation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
