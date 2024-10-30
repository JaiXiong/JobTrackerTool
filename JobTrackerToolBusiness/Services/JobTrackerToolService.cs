using Microsoft.EntityFrameworkCore;

namespace JobTracker.Business.Services
{
    public class JobTrackerToolService : DbContext
    {
        public string GetEmployerName(Guid id)
        {
            var name = "";
            return name;
        }
    }
}
