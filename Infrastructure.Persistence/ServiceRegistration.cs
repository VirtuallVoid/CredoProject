using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Settings;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Repositories
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ILoggingRepository, LoggingRepository>();
            services.AddTransient<ILoanApplicationsRepository, LoanApplicationsRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            #endregion

            services.AddOptions();
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
        }
    }
}
