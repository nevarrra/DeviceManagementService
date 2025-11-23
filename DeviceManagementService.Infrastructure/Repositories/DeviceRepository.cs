using DeviceManagementService.Domain.Models;
using DeviceManagementService.Infrastructure.Abstractions;

namespace DeviceManagementService.Infrastructure.Data.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context) => _context = context;

        
    }
}