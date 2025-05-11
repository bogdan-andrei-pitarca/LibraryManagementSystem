using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LibraryManagementSystem.Business.DTOs;

namespace LibraryManagementSystem.Business.Validators
{
    public class UserValidator
    {
        public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
        {
            public CreateUserDtoValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required")
                    .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required")
                    .EmailAddress().WithMessage("Invalid email format")
                    .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

                RuleFor(x => x.DateOfBirth)
                    .NotEmpty().WithMessage("Date of birth is required")
                    .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future");

                RuleFor(x => x.Role)
                    .IsInEnum().WithMessage("Invalid user role");
            }
        }

        public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
        {
            public UpdateUserDtoValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required")
                    .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required")
                    .EmailAddress().WithMessage("Invalid email format")
                    .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

                RuleFor(x => x.DateOfBirth)
                    .NotEmpty().WithMessage("Date of birth is required")
                    .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future");

                RuleFor(x => x.Role)
                    .IsInEnum().WithMessage("Invalid user role");
            }
        }
    }
}
