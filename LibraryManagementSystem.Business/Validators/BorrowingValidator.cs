using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Business.DTOs;
using FluentValidation;

namespace LibraryManagementSystem.Business.Validators
{
    public class CreateBorrowingDtoValidator : AbstractValidator<CreateBorrowingDto>
    {
        public CreateBorrowingDtoValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Invalid book ID");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Invalid user ID");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Due date is required")
                .GreaterThan(DateTime.Now).WithMessage("Due date must be in the future");
        }
    }
}
