using DeviceManagementService.Application.Commands;
using DeviceManagementService.Domain.Enums;
using DeviceManagementService.Domain.Models;
using DeviceManagementService.Infrastructure.Abstractions;
using FluentAssertions;
using Moq;

namespace DeviceManagementService.Tests.Application.Handlers
{
    public class CreateDeviceCommandHandlerTests
    {
        private readonly Mock<IDeviceRepository> _repositoryMock;
        private readonly CreateDeviceCommandHandler _handler;

        public CreateDeviceCommandHandlerTests()
        {
            _repositoryMock = new Mock<IDeviceRepository>();
            _handler = new CreateDeviceCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_CreatesDeviceAndReturnsId()
        {
            // Arrange
            var command = new CreateDeviceCommand("MacBook Pro", "Apple", DeviceState.Available);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<Device>(d => d.Name == "MacBook Pro" && d.Brand == "Apple" && d.State == DeviceState.Available),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithoutState_CreatesDeviceWithDefaultAvailableState()
        {
            // Arrange
            var command = new CreateDeviceCommand("iPhone 15", "Apple", null);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<Device>(d => d.State == DeviceState.Available),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithExplicitState_CreatesDeviceWithProvidedState()
        {
            // Arrange
            var command = new CreateDeviceCommand("ThinkPad", "Lenovo", DeviceState.InUse);

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(
                r => r.AddAsync(
                    It.Is<Device>(d => d.State == DeviceState.InUse),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
