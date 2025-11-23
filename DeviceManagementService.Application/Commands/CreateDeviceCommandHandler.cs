using DeviceManagementService.Domain.Models;
using MediatR;

namespace DeviceManagementService.Application.Commands
{
    internal sealed class CreateDeviceCommandHandler(IDeviceRepository deviceRepository) : IRequestHandler<CreateDeviceCommand, int>
    {
        private readonly IDeviceRepository _deviceRepository = deviceRepository;

        public async Task<int> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = new Device(
                request.Name,
                request.Brand,
                request.State ?? Domain.Enums.DeviceState.Available
            );
            await _deviceRepository.AddAsync(device);
            return device.Id;
        }
    }
}
