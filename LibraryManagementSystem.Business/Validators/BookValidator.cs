using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using LibraryManagementSystem.Business.DTOs;

namespace LibraryManagementSystem.Business.Validators
{
    public class BookValidator
    {
        public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
        {
            public CreateBookDtoValidator()
            {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title is required")
                    .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

                RuleFor(x => x.Author)
                    .NotEmpty().WithMessage("Author is required")
                    .MaximumLength(100).WithMessage("Author name cannot exceed 100 characters");

                RuleFor(x => x.Quantity)
                    .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative");
            }
        }

        public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
        {
            public UpdateBookDtoValidator()
            {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title is required")
                    .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

                RuleFor(x => x.Author)
                    .NotEmpty().WithMessage("Author is required")
                    .MaximumLength(100).WithMessage("Author name cannot exceed 100 characters");

                RuleFor(x => x.Quantity)
                    .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative");
            }
        }
    }
}
