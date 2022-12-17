using Application.Interfaces;
using Application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IServiceProvider _serviceProvider;

        public UnitOfWork(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
        }
        public ILoggingRepository LoggingRepository => _serviceProvider.GetService<ILoggingRepository>();
        public ILoanApplicationsRepository LoanApplicationsRepository => _serviceProvider.GetService<ILoanApplicationsRepository>();
        public IUserRepository UserRepository => _serviceProvider.GetService<IUserRepository>();

    }
}
