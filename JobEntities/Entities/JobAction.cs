using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEntities.Entities
{
    public class JobAction
    {
        public Guid Id { get; set; }
        public enum ActionType
        {
            Apply,
            Interview,
            Offer,
            Reject
        }
        public enum Result
        {
            Success,
            Failure
        }
    }
}
