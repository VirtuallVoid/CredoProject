using Application.Features.LoanApplicationsInfo.Queries;
using Application.Features.UsersInfo.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.LoanApplicationsInfo.RequestValidators
{
    public class GetUserLoanApplicationsQueryValidator : AbstractValidator<GetUserLoanApplicationsQuery>
    {
        public GetUserLoanApplicationsQueryValidator()
        {
            RuleFor(u => u.UserId.ToString())
               .NotEmpty().WithMessage("UserId is required.").WithErrorCode("1")
               .NotNull().WithMessage("UserId is required.").WithErrorCode("1")
               .Matches(@"^[\d]+$").WithMessage("UserId should be int").WithErrorCode("2");
        }
    }
}
