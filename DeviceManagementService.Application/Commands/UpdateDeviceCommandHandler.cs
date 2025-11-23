using DeviceManagementService.Infrastructure.Abstractions;
using MediatR;

namespace DeviceManagementService.Application.Commands
{
    internal sealed class UpdateDeviceCommandHandler(IDeviceRepository deviceRepository) : IRequestHandler<UpdateDeviceCommand, Unit>
    {
        private readonly IDeviceRepository _deviceRepository = deviceRepository;

        public async Task<Unit> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = await _deviceRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (device == null)
            {
                throw new KeyNotFoundException($"Device with Id {request.Id} not found.");
            }
            
            if (!device.CanBeModified())
            {
                throw new InvalidOperationException($"Device with Id {request.Id} cannot be updated while in-use.");
            }

            device.UpdateDevice(request.Name, request.Brand, request.State);

            await _deviceRepository.UpdateAsync(device, cancellationToken);

            return Unit.Value;
        }
    }
}
