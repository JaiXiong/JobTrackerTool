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
        public Guid UserProfileId { get; set; }
        public string? Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserProfile? UserProfile { get; set; }
    }
}
