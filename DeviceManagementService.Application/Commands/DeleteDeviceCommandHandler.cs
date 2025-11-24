using DeviceManagementService.Domain.Exceptions;
using DeviceManagementService.Infrastructure.Abstractions;
using MediatR;

namespace DeviceManagementService.Application.Commands
{
    internal sealed class DeleteDeviceCommandHandler(IDeviceRepository deviceRepository) : IRequestHandler<DeleteDeviceCommand, Unit>
    {
        private readonly IDeviceRepository _deviceRepository = deviceRepository;

        public async Task<Unit> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = await _deviceRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Device", request.Id);

            await _deviceRepository.DeleteAsync(device, cancellationToken);

            return Unit.Value;
        }
    }
}
