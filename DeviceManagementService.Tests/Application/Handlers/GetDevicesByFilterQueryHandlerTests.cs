using DeviceManagementService.Application.Queries;
using DeviceManagementService.Domain.Enums;
using DeviceManagementService.Domain.Models;
using DeviceManagementService.Infrastructure.Abstractions;
using FluentAssertions;
using Moq;

namespace DeviceManagementService.Tests.Application.Handlers
{
    public class GetDevicesByFilterQueryHandlerTests
    {
        private readonly Mock<IDeviceRepository> _repositoryMock;
        private readonly GetDeviceByFilterQueryHandler _handler;

        public GetDevicesByFilterQueryHandlerTests()
        {
            _repositoryMock = new Mock<IDeviceRepository>();
            _handler = new GetDeviceByFilterQueryHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsFilteredDevices()
        {
            // Arrange
            var devices = new List<Device>
            {
                new Device("MacBook Pro", "Apple", DeviceState.Available),
                new Device("MacBook Air", "Apple", DeviceState.Available)
            };

            var query = new GetDevicesByFilterQuery(
                Name: null,
                Brand: "Apple",
                State: DeviceState.Available,
                Page: 1,
                PageSize: 10);

            _repositoryMock
                .Setup(r => r.GetByFilterAsync(
                    query.Name,
                    query.Brand,
                    query.State,
                    query.Page,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(devices);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(d => d.Brand.Should().Be("Apple"));
        }

        [Fact]
        public async Task Handle_WithNoMatchingDevices_ReturnsEmptyCollection()
        {
            // Arrange
            var query = new GetDevicesByFilterQuery(
                Name: "NonExistent",
                Brand: null,
                State: null,
                Page: 1,
                PageSize: 10);

            _repositoryMock
                .Setup(r => r.GetByFilterAsync(
                    query.Name,
                    query.Brand,
                    query.State,
                    query.Page,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Device>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WithPagination_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var query = new GetDevicesByFilterQuery(
                Name: "Test",
                Brand: "Brand",
                State: DeviceState.InUse,
                Page: 2,
                PageSize: 5);

            _repositoryMock
                .Setup(r => r.GetByFilterAsync(
                    It.IsAny<string?>(),
                    It.IsAny<string?>(),
                    It.IsAny<DeviceState?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Device>());

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.GetByFilterAsync(
                "Test",
                "Brand",
                DeviceState.InUse,
                2,
                5,
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
