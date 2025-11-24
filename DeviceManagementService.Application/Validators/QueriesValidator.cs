using DeviceManagementService.Application.Queries;
using FluentValidation;

namespace DeviceManagementService.Application.Validators
{
    public class GetDeviceByIdQueryValidator : AbstractValidator<GetDeviceByIdQuery>
    {
        public GetDeviceByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Device ID must be greater than 0.");
        }
    }

    public class GetDevicesByFilterQueryValidator : AbstractValidator<GetDevicesByFilterQuery>
    {
        private const int MaxPageSize = 100;

        public GetDevicesByFilterQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(MaxPageSize).WithMessage($"Page size must not exceed {MaxPageSize}.");

            RuleFor(x => x.State)
                .IsInEnum().When(x => x.State.HasValue)
                .WithMessage("Invalid device state.");
        }
    }
}
