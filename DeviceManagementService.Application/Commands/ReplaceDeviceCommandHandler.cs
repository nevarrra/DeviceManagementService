using DeviceManagementService.Domain.Exceptions;
using DeviceManagementService.Infrastructure.Abstractions;
using MediatR;

namespace DeviceManagementService.Application.Commands
{
    public class ReplaceDeviceCommandHandler(IDeviceRepository deviceRepository) : IRequestHandler<ReplaceDeviceCommand, Unit>
    {
        private readonly IDeviceRepository _deviceRepository = deviceRepository;

        public async Task<Unit> Handle(ReplaceDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = await _deviceRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Device", request.Id);

            device.UpdateDevice(request.Name, request.Brand, request.State);

            await _deviceRepository.UpdateAsync(device, cancellationToken);

            return Unit.Value;
        }
    }
}
