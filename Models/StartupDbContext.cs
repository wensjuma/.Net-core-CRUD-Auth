using Microsoft.EntityFrameworkCore;


namespace WEB_API.models
{
    public class StartupDbContext : DbContext
    {
        public StartupDbContext(DbContextOptions<StartupDbContext> options) : base(options)
        {

        }
        public DbSet<Startup_db> StartupItems { get; set; }
       

        public DbSet<Users> Users { get; set; }
    }
}
