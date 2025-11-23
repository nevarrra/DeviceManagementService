using DeviceManagementService.Application.DTOs;
using DeviceManagementService.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagementService.Application.Queries
{
    internal sealed class GetDeviceByIdQueryHandler(IDeviceRepository repository)
    {
        private readonly IDeviceRepository _repository = repository;
        public async Task<DeviceDTO> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
        {
            var device = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (device == null)
            {
                throw new KeyNotFoundException($"Device with Id {request.Id} not found.");
            }

            return new DeviceDTO
            {
                Id = device.Id,
                Name = device.Name,
                Brand = device.Brand,
                State = device.State,
                CreatedAt = device.CreatedAt
            };
        }

    }
}
