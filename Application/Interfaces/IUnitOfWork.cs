using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        ILoggingRepository LoggingRepository { get; }
        ILoanApplicationsRepository LoanApplicationsRepository { get; }
        IUserRepository UserRepository { get; }
    }
}
