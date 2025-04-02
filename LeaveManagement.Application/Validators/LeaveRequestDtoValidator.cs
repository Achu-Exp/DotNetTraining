using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace LeaveManagement.Application.Validators
{
    public class LeaveRequestDtoValidator : AbstractValidator<DTO.LeaveRequestDTO>
    {
        public LeaveRequestDtoValidator()
        {
            RuleFor(request => request.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(request => request.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThan(request => request.StartDate)
                .WithMessage("End date must be after the start date.");

            RuleFor(request => request.Reason)
                .NotEmpty().WithMessage("Reason is required.")
                .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.");

            RuleFor(request => request.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than zero.");

            RuleFor(request => request.ApproverId)
                .GreaterThan(0).WithMessage("Approver ID must be greater than zero.");
        }
    }
}
