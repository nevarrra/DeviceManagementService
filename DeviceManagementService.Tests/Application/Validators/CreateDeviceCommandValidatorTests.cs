using DeviceManagementService.Application.Commands;
using DeviceManagementService.Application.Options;
using DeviceManagementService.Application.Validators;
using DeviceManagementService.Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace DeviceManagementService.Tests.Application.Validators
{
    public class CreateDeviceCommandValidatorTests
    {
        private readonly CreateDeviceCommandValidator _validator;

        public CreateDeviceCommandValidatorTests()
        {
            var options = Options.Create(new DeviceValidationOptions
            {
                MaxNameLength = 100,
                MaxBrandLength = 100
            });
            _validator = new CreateDeviceCommandValidator(options);
        }

        [Fact]
        public void Validate_WithValidCommand_ReturnsNoErrors()
        {
            // Arrange
            var command = new CreateDeviceCommand("iPhone 15", "Apple", DeviceState.Available);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_WithEmptyName_ReturnsError()
        {
            // Arrange
            var command = new CreateDeviceCommand("", "Apple", DeviceState.Available);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Validate_WithEmptyBrand_ReturnsError()
        {
            // Arrange
            var command = new CreateDeviceCommand("iPhone", "", DeviceState.Available);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Brand");
        }

        [Fact]
        public void Validate_WithNameExceedingMaxLength_ReturnsError()
        {
            // Arrange
            var longName = new string('a', 101);
            var command = new CreateDeviceCommand(longName, "Apple", DeviceState.Available);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Validate_WithNullState_ReturnsNoErrors()
        {
            // Arrange
            var command = new CreateDeviceCommand("iPhone", "Apple", null);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_WithInvalidState_ReturnsError()
        {
            // Arrange
            var command = new CreateDeviceCommand("iPhone", "Apple", (DeviceState)999);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "State");
        }
    }
}
