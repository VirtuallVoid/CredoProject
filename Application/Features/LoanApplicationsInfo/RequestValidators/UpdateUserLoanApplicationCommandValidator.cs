using Application.Features.LoanApplicationsInfo.Commands;
using FluentValidation;

namespace Application.Features.LoanApplicationsInfo.RequestValidators
{
    public class UpdateUserLoanApplicationCommandValidator : AbstractValidator<UpdateUserLoanApplicationCommand>
    {
        public UpdateUserLoanApplicationCommandValidator()
        {
            RuleFor(u => u.Id.ToString())
               .NotEmpty().WithMessage("Loan Id is required.").WithErrorCode("1")
               .NotNull().WithMessage("Loan Id is required.").WithErrorCode("1")
               .Matches(@"^[\d]+$").WithMessage("Loan Id should be int").WithErrorCode("2");

            RuleFor(u => u.LoanTypeId.ToString())
               .Matches(@"^[\d]+$").WithMessage("LoanTypeId should be int").WithErrorCode("2");

            RuleFor(u => u.StatusId.ToString())
               .Matches(@"^[\d]+$").WithMessage("StatusId should be int").WithErrorCode("2");

            RuleFor(p => p.UserId.ToString())
               .NotEmpty().WithMessage("UserId is required.").WithErrorCode("1")
               .NotNull().WithMessage("UserId is required.").WithErrorCode("1")
               .Matches(@"^[\d]+$").WithMessage("UserId should be int").WithErrorCode("2");

            RuleFor(p => p.Currency)
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
