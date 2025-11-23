using DeviceManagementService.Application.DTOs;
using DeviceManagementService.Infrastructure.Abstractions;
using MediatR;

namespace DeviceManagementService.Application.Queries
{
    internal sealed class GetDeviceByFilterQueryHandler(IDeviceRepository repository) : IRequestHandler<GetDevicesByFilterQuery, IEnumerable<DeviceDTO>>
    {
        private readonly IDeviceRepository _repository = repository;

        public async Task<IEnumerable<DeviceDTO>> Handle(GetDevicesByFilterQuery request, CancellationToken cancellationToken)
        {
            var devices = await _repository.GetByFilterAsync(
                request.Name,
                request.Brand,
                request.State,
                request.Page,
                request.PageSize,
                cancellationToken
            );

            return devices.Select(d => new DeviceDTO
            {
                Id = d.Id,
                Name = d.Name,
                Brand = d.Brand,
                State = d.State,
                CreatedAt = d.CreatedAt
            }).ToList();
        }
    }
}