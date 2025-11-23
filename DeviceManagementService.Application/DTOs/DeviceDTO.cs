using DeviceManagementService.Domain.Enums;

namespace DeviceManagementService.Application.DTOs
{
    public record DeviceDTO( 
        int Id,
        string Name,
        string Brand,
        DeviceState State,
        DateTime CreatedAt);
}
