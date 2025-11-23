using DeviceManagementService.Infrastructure.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagementService.Application.Commands
{
    internal sealed class DeleteDeviceCommandHandler(IDeviceRepository deviceRepository) : IRequestHandler<DeleteDeviceCommand, Unit>
    {
        private readonly IDeviceRepository _deviceRepository = deviceRepository;

        public async Task<Unit> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = await _deviceRepository.GetByIdAsync(request.Id, cancellationToken);

            if (device == null)
            {
                throw new KeyNotFoundException($"Device with Id {request.Id} not found.");
            }

            if (!device.CanBeModified())
            {
                throw new InvalidOperationException($"In-use device with Id {request.Id} cannot be deleted.");
            }

            await _deviceRepository.DeleteAsync(device, cancellationToken);

            return Unit.Value;
        }
    }
}
