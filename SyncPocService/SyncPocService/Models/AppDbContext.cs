using Microsoft.EntityFrameworkCore;

namespace SyncPocService.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.FirstName)
                    .HasMaxLength(20)
                    .IsRequired();

                e.Property(x => x.LastName)
                    .HasMaxLength(20);

                e.Property(x => x.Email)
                    .HasMaxLength(100)
                    .IsRequired();

                e.Property(x => x.Phone)
                  .HasMaxLength(20);

                e.Property(x => x.Address)
                   .HasMaxLength(200);

            });

            modelBuilder.Entity<Employee>();
        }
    }
}
