using DeviceManagementService.Application.Queries;
using DeviceManagementService.Application.Validators;
using DeviceManagementService.Domain.Enums;
using FluentAssertions;

namespace DeviceManagementService.Tests.Application.Validators
{
    public class GetDevicesByFilterQueryValidatorTests
    {
        private readonly GetDevicesByFilterQueryValidator _validator;

        public GetDevicesByFilterQueryValidatorTests()
        {
            _validator = new GetDevicesByFilterQueryValidator();
        }

        [Fact]
        public void Validate_WithValidQuery_ReturnsNoErrors()
        {
            // Arrange
            var query = new GetDevicesByFilterQuery(
                Name: "iPhone",
                Brand: "Apple",
                State: DeviceState.Available,
                Page: 1,
                PageSize: 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WithPageZero_ReturnsError()
        {
            // Arrange
            var query = new GetDevicesByFilterQuery(Page: 0, PageSize: 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Page");
        }

        [Fact]
        public void Validate_WithNegativePage_ReturnsError()
        {
            // Arrange
            var query = new GetDevicesByFilterQuery(Page: -1, PageSize: 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Page");
        }

        [Fact]
        public void Validate_WithPageSizeZero_ReturnsError()
        {
            // Arrange
            var query = new GetDevicesByFilterQuery(Page: 1, PageSize: 0);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "PageSize");
        }

        [Fact]
        public void Validate_WithPageSizeExceedingMax_ReturnsError()
        {
            // Arrange
            var query = new GetDevicesByFilterQuery(Page: 1, PageSize: 101);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "PageSize");
        }

        [Fact]
        public void Validate_WithMaxPageSize_ReturnsNoErrors()
        {
            // Arrange
            var query = new GetDevicesByFilterQuery(Page: 1, PageSize: 100);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WithInvalidState_ReturnsError()
        {
            // Arrange
            var query = new GetDevicesByFilterQuery(State: (DeviceState)999, Page: 1, PageSize: 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "State");
        }
    }
}
