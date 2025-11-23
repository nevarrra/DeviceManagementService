using DeviceManagementService.Application.DTOs;
using DeviceManagementService.Domain.Enums;
using MediatR;

namespace DeviceManagementService.Application.Queries
{
    public record GetDevicesByFilterQuery(
        string? Name = null,
        string? Brand = null,
        DeviceState? State = null,
        int Page = 1,
        int PageSize = 20) : IRequest<IEnumerable<DeviceDTO>>;
}
