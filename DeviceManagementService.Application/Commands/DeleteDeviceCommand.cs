using MediatR;

namespace DeviceManagementService.Application.Commands
{
    public record DeleteDeviceCommand(int Id) : IRequest<Unit>;
}
