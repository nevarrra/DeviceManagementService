using DeviceManagementService.Domain.Enums;
using MediatR;

namespace DeviceManagementService.Application.Commands
{
    public record UpdateDeviceCommand(
        int Id,
        string Name = null,
        string Brand = null,
        DeviceState? State = null) : IRequest<Unit>;
}
