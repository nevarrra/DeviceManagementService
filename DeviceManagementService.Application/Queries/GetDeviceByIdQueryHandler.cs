using DeviceManagementService.Application.DTOs;
using DeviceManagementService.Domain.Exceptions;
using DeviceManagementService.Infrastructure.Abstractions;
using MediatR;

namespace DeviceManagementService.Application.Queries
{
    internal sealed class GetDeviceByIdQueryHandler(IDeviceRepository repository) : IRequestHandler<GetDeviceByIdQuery, DeviceDTO?>
    {
        private readonly IDeviceRepository _repository = repository;
        public async Task<DeviceDTO?> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var device = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Device", request.Id);

            return new DeviceDTO(
                device.Id,
                device.Name,
                device.Brand,
                device.State,
                device.CreatedAt
            );
        }
    }
}
