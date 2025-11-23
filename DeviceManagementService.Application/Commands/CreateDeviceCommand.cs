using DeviceManagementService.Domain.Enums;
using MediatR;

namespace DeviceManagementService.Application.Commands
{
    public record CreateDeviceCommand(
        string Name,
        string Brand,
        DeviceState? State = null
    ) : IRequest<int>;
}
