using Application.Features.UsersInfo.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.UsersInfo.RequestValidators
{
    public class LoginCustomerCommandValidator : AbstractValidator<LoginCustomerCommand>
    {
        public LoginCustomerCommandValidator()
        {
            RuleFor(u => u.Username)
               .NotEmpty().WithMessage("Username is required.").WithErrorCode("1")
               .NotNull().WithMessage("Username is required.").WithErrorCode("1")
               .MaximumLength(20).WithMessage("Username exceeds 20 chars").WithErrorCode("3")
               .MinimumLength(5).WithMessage("Username is less than 5 chars").WithErrorCode("4");


            RuleFor(p => p.Password)
               .NotEmpty().WithMessage("Password is required.").WithErrorCode("1")
               .NotNull().WithMessage("Password is required.").WithErrorCode("1");
        }
    }
}
