using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LeaveManagement.Application.DTO;

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
