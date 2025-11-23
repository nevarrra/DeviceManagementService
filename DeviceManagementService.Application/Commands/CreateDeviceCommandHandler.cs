using DeviceManagementService.Domain.Enums;
using DeviceManagementService.Domain.Models;
using DeviceManagementService.Infrastructure.Abstractions;
using MediatR;

namespace DeviceManagementService.Application.Commands
{
    internal sealed class CreateDeviceCommandHandler(IDeviceRepository deviceRepository) : IRequestHandler<CreateDeviceCommand, int>
    {
        private readonly IDeviceRepository _deviceRepository = deviceRepository;

        public async Task<int> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = request.State.HasValue
                ? new Device(request.Name, request.Brand, request.State.Value)
                : new Device(request.Name, request.Brand);

            await _deviceRepository.AddAsync(device, cancellationToken);

            return device.Id;
        }
    }
}
