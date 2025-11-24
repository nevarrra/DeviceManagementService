using DeviceManagementService.Application.Commands;
using DeviceManagementService.Domain.Enums;
using DeviceManagementService.Domain.Exceptions;
using DeviceManagementService.Domain.Models;
using DeviceManagementService.Infrastructure.Abstractions;
using FluentAssertions;
using Moq;

namespace DeviceManagementService.Tests.Application.Handlers
{
    public class UpdateDeviceCommandHandlerTests
    {
        private readonly Mock<IDeviceRepository> _repositoryMock;
        private readonly UpdateDeviceCommandHandler _handler;

        public UpdateDeviceCommandHandlerTests()
        {
            _repositoryMock = new Mock<IDeviceRepository>();
            _handler = new UpdateDeviceCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDeviceExists_UpdatesDevice()
        {
            // Arrange
            var existingDevice = new Device("Old Name", "Old Brand", DeviceState.Available);
            var command = new UpdateDeviceCommand(1, "New Name", "New Brand", DeviceState.Inactive);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingDevice);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(existingDevice, It.IsAny<CancellationToken>()), Times.Once);
            existingDevice.Name.Should().Be("New Name");
            existingDevice.Brand.Should().Be("New Brand");
            existingDevice.State.Should().Be(DeviceState.Inactive);
        }

        [Fact]
        public async Task Handle_WhenDeviceNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var command = new UpdateDeviceCommand(999, "Name", "Brand", null);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Device?)null);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*999*");
        }

        [Fact]
        public async Task Handle_PartialUpdate_OnlyUpdatesProvidedFields()
        {
            // Arrange
            var existingDevice = new Device("Original", "Original Brand", DeviceState.Available);
            var command = new UpdateDeviceCommand(1, "Updated Name", null, null);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingDevice);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingDevice.Name.Should().Be("Updated Name");
            existingDevice.Brand.Should().Be("Original Brand");
            existingDevice.State.Should().Be(DeviceState.Available);
        }
    }
}
