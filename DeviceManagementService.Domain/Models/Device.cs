using DeviceManagementService.Domain.Enums;

namespace DeviceManagementService.Domain.Models
{
    public class Device
    {
        public int Id { get; set; }

        public string Name { get; private set; }

        public string Brand { get; private set; }

        public DeviceState State { get; private set; }

        public DateTime CreatedAt { get; private set; }

        // Constructor for EF Core
        private Device() { }

        public Device(string name, string brand, DeviceState defaultState = DeviceState.Available)
        {
            Name = name;
            Brand = brand;
            State = defaultState;
            CreatedAt = DateTime.UtcNow;
        }

        // Constructor for seeding data with specific Ids
        public Device(int id, string name, string brand, DeviceState defaultState = DeviceState.Available)
        {
            Id = id;
            Name = name;
            Brand = brand;
            State = defaultState;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateDevice(string? name, string? brand, DeviceState? state)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;
            if (!string.IsNullOrWhiteSpace(brand))
                Brand = brand;
            if (state != null)
                State = (DeviceState)state;
        }

        public bool CanBeModified()
        {
            return State != DeviceState.InUse;
        }
    }
}
