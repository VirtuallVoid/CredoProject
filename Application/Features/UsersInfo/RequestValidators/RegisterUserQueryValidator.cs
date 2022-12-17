using Application.Features.UsersInfo.Commands;
using Application.Features.UsersInfo.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.UsersInfo.RequestValidators
{
    public class RegisterUserQueryValidator : AbstractValidator<RegisterUserQuery>
    {
        public RegisterUserQueryValidator()
        {
            RuleFor(u => u.UserName)
               .NotEmpty().WithMessage("Username is required.").WithErrorCode("1")
               .NotNull().WithMessage("Username is required.").WithErrorCode("1")
               .MaximumLength(20).WithMessage("Username exceeds 20 chars").WithErrorCode("3")
               .MinimumLength(5).WithMessage("Username is less than 5 chars").WithErrorCode("4");

            RuleFor(p => p.Password)
               .NotEmpty().WithMessage("Password is required.").WithErrorCode("1")
               .NotNull().WithMessage("Password is required.").WithErrorCode("1");

            RuleFor(u => u.Email)
               .NotEmpty().WithMessage("Email is required.").WithErrorCode("1")
               .NotNull().WithMessage("Email is required.").WithErrorCode("1")
               .MinimumLength(5).WithMessage("Email is less than 5 chars").WithErrorCode("4");

            RuleFor(u => u.Mobile)
               .NotEmpty().WithMessage("Mobile Number is required.").WithErrorCode("1")
               .NotNull().WithMessage("Mobile Number is required.").WithErrorCode("1")
               .MinimumLength(9).WithMessage("Mobile Number is less than 9 chars").WithErrorCode("4");

            RuleFor(u => u.PersonalNumber)
               .NotEmpty().WithMessage("PersonalNumber is required.").WithErrorCode("1")
               .NotNull().WithMessage("PersonalNumber is required.").WithErrorCode("1")
               .MaximumLength(11).WithMessage("PersonalNumber is more than 11 chars").WithErrorCode("3")
               .MinimumLength(11).WithMessage("PersonalNumber is less than 11 chars").WithErrorCode("4");

            RuleFor(u => u.FullName)
               .NotEmpty().WithMessage("FullName is required.").WithErrorCode("1")
               .NotNull().WithMessage("FullName is required.").WithErrorCode("1");
        }
    }
}
