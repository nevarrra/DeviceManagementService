namespace DeviceManagementService.Application.Options
{
    public class DeviceValidationOptions
    {
        public const string SectionName = "Device";

        public int MaxNameLength { get; set; } = 200;

        public int MaxBrandLength { get; set; } = 100;
    }
}