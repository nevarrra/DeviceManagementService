using DeviceManagementService.Domain.Models;

namespace DeviceManagementService.Infrastructure.Abstractions
{
    public interface IDeviceRepository
    {
        Task AddAsync(Device device, CancellationToken cancellationToken);

        Task<Device?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<Device>> GetByFilterAsync(
            string? name,
            string? brand,
            Domain.Enums.DeviceState? state,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

        Task UpdateAsync(Device device, CancellationToken cancellationToken);

        Task DeleteAsync(Device device, CancellationToken cancellationToken);
    }
}
