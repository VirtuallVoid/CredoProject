using Application.Features.LoanApplicationsInfo.Commands;
using Application.Features.UsersInfo.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.LoanApplicationsInfo.RequestValidators
{
    public class DeleteUserLoanApplicationCommandValidator : AbstractValidator<DeleteUserLoanApplicationCommand>
    {
        public DeleteUserLoanApplicationCommandValidator()
        {
            RuleFor(u => u.Id.ToString())
               .NotEmpty().WithMessage("Loan Id is required.").WithErrorCode("1")
               .NotNull().WithMessage("Loan Id is required.").WithErrorCode("1")
               .Matches(@"^[\d]+$").WithMessage("Loan Id should be int").WithErrorCode("2");

            RuleFor(p => p.UserId.ToString())
               .NotEmpty().WithMessage("UserId is required.").WithErrorCode("1")
               .NotNull().WithMessage("UserId is required.").WithErrorCode("1")
               .Matches(@"^[\d]+$").WithMessage("UserId should be int").WithErrorCode("2");
        }
    }
}
