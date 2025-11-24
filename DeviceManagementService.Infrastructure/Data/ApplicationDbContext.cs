using DeviceManagementService.Domain.Models;
using DeviceManagementService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementService.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Brand).IsRequired().HasMaxLength(100);
                entity.Property(e => e.State).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Seed data with fixed dates (required by EF Core)
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Device>().HasData(
                new { Id = 1, Name = "MacBook Pro 16", Brand = "Apple", State = DeviceState.Available, CreatedAt = seedDate },
                new { Id = 2, Name = "ThinkPad X1 Carbon", Brand = "Lenovo", State = DeviceState.InUse, CreatedAt = seedDate },
                new { Id = 3, Name = "Surface Laptop 5", Brand = "Microsoft", State = DeviceState.Available, CreatedAt = seedDate },
                new { Id = 4, Name = "WashPRO", Brand = "Siemens", State = DeviceState.Inactive, CreatedAt = seedDate },
                new { Id = 5, Name = "BestWifiFridge", Brand = "Samsung", State = DeviceState.Available, CreatedAt = seedDate }
            );
        }
    }
}