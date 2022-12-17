using Application.Features.LoanApplicationsInfo.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.LoanApplicationsInfo.RequestValidators
{
    public class GetUserLoanApplicationByIdQueryValidator : AbstractValidator<GetUserLoanApplicationByIdQuery>
    {
        public GetUserLoanApplicationByIdQueryValidator()
        {
            RuleFor(u => u.UserId.ToString())
               .NotEmpty().WithMessage("UserId is required.").WithErrorCode("1")
               .NotNull().WithMessage("UserId is required.").WithErrorCode("1")
               .Matches(@"^[\d]+$").WithMessage("UserId should be int").WithErrorCode("2");

            RuleFor(u => u.LoanId.ToString())
               .NotEmpty().WithMessage("LoanId is required.").WithErrorCode("1")
               .NotNull().WithMessage("LoanId is required.").WithErrorCode("1")
               .Matches(@"^[\d]+$").WithMessage("LoanId should be int").WithErrorCode("2");
        }
    }
}
