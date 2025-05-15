using FluentValidation;

namespace LeaveManagement.Application.Validators
{
    public class ManagerDtoValidator : AbstractValidator<DTO.ManagerDTO>
    {
        public ManagerDtoValidator()
        {
            RuleFor(manager => manager.User)
           .NotNull().WithMessage("User details are required.")
           .SetValidator(new UserDtoValidator());
        }
    }
}
