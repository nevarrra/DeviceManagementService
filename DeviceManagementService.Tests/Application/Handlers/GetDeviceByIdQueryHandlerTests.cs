using DeviceManagementService.Application.Queries;
using DeviceManagementService.Domain.Enums;
using DeviceManagementService.Domain.Exceptions;
using DeviceManagementService.Domain.Models;
using DeviceManagementService.Infrastructure.Abstractions;
using FluentAssertions;
using Moq;

namespace DeviceManagementService.Tests.Application.Handlers
{
    public class GetDeviceByIdQueryHandlerTests
    {
        private readonly Mock<IDeviceRepository> _repositoryMock;
        private readonly GetDeviceByIdQueryHandler _handler;

        public GetDeviceByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IDeviceRepository>();
            _handler = new GetDeviceByIdQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenDeviceExists_ReturnsDeviceDTO()
        {
            // Arrange
            var device = new Device("MacBook Pro", "Apple", DeviceState.Available);
            var query = new GetDeviceByIdQuery(1);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(device);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("MacBook Pro");
            result.Brand.Should().Be("Apple");
            result.State.Should().Be(DeviceState.Available);
        }

        [Fact]
        public async Task Handle_WhenDeviceNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var query = new GetDeviceByIdQuery(999);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Device?)null);

            // Act
            var act = () => _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*999*");
        }
    }
}
