using Application.Features.LoanApplicationsInfo.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.LoanApplicationsInfo.RequestValidators
{
    public class InsertUserLoanApplicationCommandValidator : AbstractValidator<InsertUserLoanApplicationCommand>
    {
        public InsertUserLoanApplicationCommandValidator()
        {
            RuleFor(u => u.LoanTypeId.ToString())
               .NotEmpty().WithMessage("LoanTypeId is required.").WithErrorCode("1")
               .NotNull().WithMessage("LoanTypeId is required.").WithErrorCode("1")
               .Matches(@"^[\d]+$").WithMessage("LoanTypeId should be int").WithErrorCode("2");

            RuleFor(u => u.StatusId.ToString())
               .NotEmpty().WithMessage("StatusId is required.").WithErrorCode("1")
               .NotNull().WithMessage("StatusId is required.").WithErrorCode("1")
               .Matches(@"^[\d]+$").WithMessage("StatusId should be int").WithErrorCode("2");

            RuleFor(p => p.UserId.ToString())
               .NotEmpty().WithMessage("UserId is required.").WithErrorCode("1")
               .NotNull().WithMessage("UserId is required.").WithErrorCode("1")
               .Matches(@"^[\d]+$").WithMessage("UserId should be int").WithErrorCode("2");

            RuleFor(p => p.Amount.ToString())
               .NotEmpty().WithMessage("Amount is required.").WithErrorCode("1")
               .NotNull().WithMessage("Amount is required.").WithErrorCode("1");

            RuleFor(p => p.Currency)
               .NotEmpty().WithMessage("Currency is required.").WithErrorCode("1")
               .NotNull().WithMessage("Currency is required.").WithErrorCode("1")
               .Must(IsCurrencyCorrect).WithMessage("Incorrect Currency").WithErrorCode("16");
        }

        private bool IsCurrencyCorrect(string currency)
        {
            if (currency.Equals("GEL") || currency.Equals("USD") || currency.Equals("EUR"))
                return true;
            return false;
        }
    }
}
