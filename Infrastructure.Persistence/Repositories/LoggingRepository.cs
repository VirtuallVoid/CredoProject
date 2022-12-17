using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories
{
    public class LoggingRepository : GenericRepository<LogTable>, ILoggingRepository
    {
        public LoggingRepository(IConfiguration configuration) : base(configuration) { }
    }
}
