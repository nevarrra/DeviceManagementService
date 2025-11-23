using DeviceManagementService.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagementService.Application.Commands
{
    public record UpdateDeviceCommand(
        int Id,
        string Name = null,
        string Brand = null,
        DeviceState? State = null) : IRequest<Unit>;
}
