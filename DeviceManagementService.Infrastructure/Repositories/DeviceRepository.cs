using DeviceManagementService.Domain.Models;
using DeviceManagementService.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementService.Infrastructure.Data.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context) => _context = context;

        public async Task<Device?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Devices.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Device>> GetByFilterAsync(
            string? name,
            string? brand,
            Domain.Enums.DeviceState? state,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var query = _context.Devices.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(d => d.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(d => d.Brand.Contains(brand));

            if (state.HasValue)
                query = query.Where(d => d.State == state.Value);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Device device, CancellationToken cancellationToken)
        {
            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Device device, CancellationToken cancellationToken)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Device device, CancellationToken cancellationToken)
        {
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}