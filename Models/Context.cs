using Microsoft.EntityFrameworkCore;

namespace Dashboard.Models
{
    public class DashboardContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}