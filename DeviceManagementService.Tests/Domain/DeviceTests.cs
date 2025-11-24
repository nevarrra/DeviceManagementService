using DeviceManagementService.Domain.Enums;
using DeviceManagementService.Domain.Models;
using FluentAssertions;

namespace DeviceManagementService.Tests.Domain
{
    public class DeviceTests
    {
        [Fact]
        public void Constructor_WithNameAndBrand_SetsDefaultStateToAvailable()
        {
            // Arrange & Act
            var device = new Device("iPhone 15", "Apple");

            // Assert
            device.Name.Should().Be("iPhone 15");
            device.Brand.Should().Be("Apple");
            device.State.Should().Be(DeviceState.Available);
            device.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Constructor_WithExplicitState_SetsProvidedState()
        {
            // Arrange & Act
            var device = new Device("ThinkPad", "Lenovo", DeviceState.InUse);

            // Assert
            device.State.Should().Be(DeviceState.InUse);
        }

        [Fact]
        public void UpdateDevice_WhenAvailable_UpdatesAllFields()
        {
            // Arrange
            var device = new Device("Old Name", "Old Brand", DeviceState.Available);

            // Act
            device.UpdateDevice("New Name", "New Brand", DeviceState.Inactive);

            // Assert
            device.Name.Should().Be("New Name");
            device.Brand.Should().Be("New Brand");
            device.State.Should().Be(DeviceState.Inactive);
        }

        [Fact]
        public void UpdateDevice_WhenAvailable_WithPartialUpdate_OnlyUpdatesProvidedFields()
        {
            // Arrange
            var device = new Device("Original Name", "Original Brand", DeviceState.Available);

            // Act
            device.UpdateDevice("Updated Name", null, null);

            // Assert
            device.Name.Should().Be("Updated Name");
            device.Brand.Should().Be("Original Brand");
            device.State.Should().Be(DeviceState.Available);
        }

        [Fact]
        public void UpdateDevice_WhenInUse_AndChangingNameAndBrand_ThrowsException()
        {
            // Arrange
            var device = new Device("Laptop", "Dell", DeviceState.InUse);

            // Act
            var act = () => device.UpdateDevice("New Name", "New Brand", null);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Cannot modify, delete or replace a device that is currently in use.");
        }

        [Fact]
        public void UpdateDevice_WhenInUse_CanChangeStateIfNameAndBrandMatch()
        {
            // Arrange
            var device = new Device("Laptop", "Dell", DeviceState.InUse);

            // Act - pass same name and brand to avoid modification check
            device.UpdateDevice("Laptop", "Dell", DeviceState.Available);

            // Assert
            device.State.Should().Be(DeviceState.Available);
        }

        [Fact]
        public void UpdateDevice_WhenInactive_CanBeModified()
        {
            // Arrange
            var device = new Device("Old Device", "Old Brand", DeviceState.Inactive);

            // Act
            device.UpdateDevice("Refurbished Device", "New Brand", DeviceState.Available);

            // Assert
            device.Name.Should().Be("Refurbished Device");
            device.Brand.Should().Be("New Brand");
            device.State.Should().Be(DeviceState.Available);
        }
    }
}
