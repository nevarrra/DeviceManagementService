using FluentValidation;
using DeviceManagementService.Application.Commands;
using DeviceManagementService.Application.Options;
using Microsoft.Extensions.Options;
using DeviceManagementService.Infrastructure.Abstractions;

namespace DeviceManagementService.Application.Validators
{
    // TODO: Further improvements:
    // Create a parent class for commands and validators
    // Inherit common validator class to apply repetitive rules for name, brand, state etc.
    // This will reduce redundancy and improve maintainability.
    public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
    {
        public CreateDeviceCommandValidator(IOptions<DeviceValidationOptions> options)
        {
            var validationOptions = options.Value;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Device name is required.")
                .MaximumLength(validationOptions.MaxNameLength)
                .WithMessage($"Device name must not exceed {validationOptions.MaxNameLength} characters.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Device brand is required.")
                .MaximumLength(validationOptions.MaxBrandLength)
                .WithMessage($"Device brand must not exceed {validationOptions.MaxBrandLength} characters.");

            RuleFor(x => x.State)
                .IsInEnum().When(x => x.State.HasValue)
                .WithMessage("Invalid device state.");
        }
    }

    public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
    {
        private readonly IDeviceRepository _repository;
        public UpdateDeviceCommandValidator(IDeviceRepository repository, IOptions<DeviceValidationOptions> options)
        {
            _repository = repository;

            var validationOptions = options.Value;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Device ID must be greater than 0.");

            RuleFor(x => x)
                .Must(x => x.Name is not null || x.Brand is not null || x.State.HasValue)
                .WithMessage("At least one field (Name, Brand, or State) must be provided for update.");

            RuleFor(x => x.Name)
                .MaximumLength(validationOptions.MaxNameLength).When(x => x.Name is not null)
                .WithMessage($"Device name must not exceed {validationOptions.MaxNameLength} characters.");

            RuleFor(x => x.Brand)
                .MaximumLength(validationOptions.MaxBrandLength).When(x => x.Brand is not null)
                .WithMessage($"Device brand must not exceed {validationOptions.MaxBrandLength} characters.");

            RuleFor(x => x.State)
                .IsInEnum().When(x => x.State.HasValue)
                .WithMessage("Invalid device state.");
        }
    }

    public class ReplaceDeviceCommandValidator : AbstractValidator<ReplaceDeviceCommand>
    {
        private readonly IDeviceRepository _repository;
        public ReplaceDeviceCommandValidator(IDeviceRepository repository, IOptions<DeviceValidationOptions> options)
        {
            _repository = repository;

            var validationOptions = options.Value;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Device ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Device name is required.")
                .MaximumLength(validationOptions.MaxNameLength)
                .WithMessage($"Device name must not exceed {validationOptions.MaxNameLength} characters.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Device brand is required.")
                .MaximumLength(validationOptions.MaxBrandLength)
                .WithMessage($"Device brand must not exceed {validationOptions.MaxBrandLength} characters.");

            RuleFor(x => x.State)
                .IsInEnum().WithMessage("Invalid device state.");
        }
    }

    public class DeleteDeviceCommandValidator : AbstractValidator<DeleteDeviceCommand>
    {
        private readonly IDeviceRepository _repository;
        public DeleteDeviceCommandValidator(IDeviceRepository repository)
        {
            _repository = repository;
               
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Device ID must be greater than 0.");
        }
    }
}