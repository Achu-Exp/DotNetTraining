using FluentValidation;

namespace LeaveManagement.Application.Validators
{
    public class DepartmentDtoValidator : AbstractValidator<DTO.DepartmentDTO>
    {
        public DepartmentDtoValidator()
        {
            RuleFor(department => department.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(department => department.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
