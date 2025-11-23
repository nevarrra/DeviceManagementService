using DeviceManagementService.Application.DTOs;
using DeviceManagementService.Infrastructure.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagementService.Application.Queries
{
    internal sealed class GetDeviceByIdQueryHandler(IDeviceRepository repository) : IRequestHandler<GetDeviceByIdQuery, DeviceDTO?>
    {
        private readonly IDeviceRepository _repository = repository;
        public async Task<DeviceDTO?> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var device = await _repository.GetByIdAsync(request.Id, cancellationToken);

            return device == null
                ? throw new KeyNotFoundException($"Device with Id {request.Id} not found.")
                : new DeviceDTO
              (
                device.Id,
                device.Name,
                device.Brand,
                device.State,
                device.CreatedAt
            );
        }

    }
}
