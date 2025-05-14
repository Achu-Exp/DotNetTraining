using FluentValidation;

namespace LeaveManagement.Application.Validators
{
    public class UserDtoValidator : AbstractValidator<DTO.UserDTO>
    {
        public UserDtoValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters long."); ;

            RuleFor(x=>x.Email).EmailAddress().WithMessage("Invalid email format.")
                .NotEmpty().WithMessage("Email is required");

            RuleFor(x => x.Address).MaximumLength(255)
                .WithMessage("Length should be less than 255 characters");

            RuleFor(user => user.DepartmentId)
                .NotNull().WithMessage("Department ID is required.");
        }
    }
}
