using DeviceManagementService.Infrastructure.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagementService.Application.Commands
{
    public class ReplaceDeviceCommandHandler(IDeviceRepository deviceRepository) : IRequestHandler<UpdateDeviceCommand, Unit>
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
                throw new InvalidOperationException($"Device with Id {request.Id} cannot be replaced while in-use.");
            }

            device.UpdateDevice(request.Name, request.Brand, request.State);

            await _deviceRepository.UpdateAsync(device, cancellationToken);

            return Unit.Value;
        }
    }
}
