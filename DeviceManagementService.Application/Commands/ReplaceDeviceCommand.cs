using DeviceManagementService.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagementService.Application.Commands
{
    public record ReplaceDeviceCommand(
        int Id,
        string Name,
        string Brand,
        DeviceState State
    ) : IRequest<Unit>;
}
