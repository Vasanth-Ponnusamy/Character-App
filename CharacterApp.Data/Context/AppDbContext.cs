using CharacterApp.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<LocationInfo> Locations { get; set; }
        public DbSet<Episode> Episodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>()
                .OwnsOne(c => c.Origin);

            modelBuilder.Entity<Character>()
                .OwnsOne(c => c.Location);

            base.OnModelCreating(modelBuilder);
        }
    }

}
