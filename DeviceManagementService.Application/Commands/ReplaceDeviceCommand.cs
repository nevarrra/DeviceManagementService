using DeviceManagementService.Domain.Enums;
using MediatR;

namespace DeviceManagementService.Application.Commands
{
    public record ReplaceDeviceCommand(
        int Id,
        string Name,
        string Brand,
        DeviceState State
    ) : IRequest<Unit>;
}
