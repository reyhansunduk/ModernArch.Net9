using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(b =>
            {
                b.ToTable("Customers");
                b.HasKey(x => x.Id);
                b.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
                b.Property(x => x.LastName).HasMaxLength(100).IsRequired();
                b.Property(x => x.Email).HasMaxLength(200).IsRequired();
                b.Property(x => x.IsActive).HasDefaultValue(true);
            });
        }
    }
}
