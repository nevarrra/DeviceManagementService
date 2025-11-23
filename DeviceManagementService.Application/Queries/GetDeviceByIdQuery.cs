using DeviceManagementService.Application.DTOs;
using MediatR;

namespace DeviceManagementService.Application.Queries
{
    public record GetDeviceByIdQuery(int Id) : IRequest<DeviceDTO?>;
}
