using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RepairTracker.Areas.Identity.Data
{
    public class RepairTrackerIdentityContext : IdentityDbContext<RepairTrackerUser>
    {
        public RepairTrackerIdentityContext(DbContextOptions<RepairTrackerIdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
        }
    }
}
