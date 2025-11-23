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

            // Seed data
            var seedDevices = new List<Device>
            {
                new Device(1, "MacBook Pro 16", "Apple", DeviceState.Available),
                new Device(2, "ThinkPad X1 Carbon", "Lenovo", DeviceState.InUse),
                new Device(3, "Surface Laptop 5", "Microsoft", DeviceState.Available),
                new Device(4, "WashPRO", "Siemens", DeviceState.Inactive),
                new Device(5, "BestWifiFridge", "Samsung", DeviceState.Available)
            };

            modelBuilder.Entity<Device>().HasData(seedDevices);
        }
    }
}