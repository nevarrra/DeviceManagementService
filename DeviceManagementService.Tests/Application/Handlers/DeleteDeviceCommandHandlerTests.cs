using DeviceManagementService.Application.Commands;
using DeviceManagementService.Domain.Enums;
using DeviceManagementService.Domain.Exceptions;
using DeviceManagementService.Domain.Models;
using DeviceManagementService.Infrastructure.Abstractions;
using FluentAssertions;
using Moq;

namespace DeviceManagementService.Tests.Application.Handlers
{
    public class DeleteDeviceCommandHandlerTests
    {
        private readonly Mock<IDeviceRepository> _repositoryMock;
        private readonly DeleteDeviceCommandHandler _handler;

        public DeleteDeviceCommandHandlerTests()
        {
            _repositoryMock = new Mock<IDeviceRepository>();
            _handler = new DeleteDeviceCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDeviceExists_DeletesDevice()
        {
            // Arrange
            var existingDevice = new Device("Test Device", "Test Brand", DeviceState.Available);
            var command = new DeleteDeviceCommand(1);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingDevice);

            _repositoryMock
                .Setup(r => r.DeleteAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(existingDevice, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDeviceNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var command = new DeleteDeviceCommand(999);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Device?)null);

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*999*");
        }
    }
}
