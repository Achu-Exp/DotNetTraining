using FluentValidation;
using LeaveManagementSystem.Application.Validators;

namespace LeaveManagement.Application.Validators
{
    public class EmployeeDtoValidator : AbstractValidator<DTO.EmployeeDTO>
    {
        public EmployeeDtoValidator()
        {
            RuleFor(x => x.User)
                .NotEmpty().WithMessage("User details are required")
                .SetValidator(new UsersDtoValidators());

            RuleFor(x => x.ManagerId)
                .GreaterThan(0).When(e => e.ManagerId.HasValue)
                .WithMessage("ManagerId must be a valid positive number.");
        }
    }
}
