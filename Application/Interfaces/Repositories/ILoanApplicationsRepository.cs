using Application.DTOs.LoanApplication.Request;
using Application.DTOs.LoanApplication.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ILoanApplicationsRepository : IGenericRepository<LoanApplications>
    {
        Task<IEnumerable<LoanApplications>> GetUserLoanApplicationsAsync(int userId); 
        Task<LoanApplicationResponse> GetUserLoanApplicationByIdAsync(int userId, int loanId);
        Task<IEnumerable<StatusTypes>> GetApplicationStatusTypesAsync();
        Task<IEnumerable<LoanTypes>> GetApplicationLoanTypesAsync();
        Task<int> InsertUserLoanApplicationAsync(LoanApplicationRequest loan);
        Task<int> UpdateUserLoanApplicationAsync(LoanApplicationRequest loan);
        Task<int> DeleteUserLoanApplicationAsync(int userId, int loanId);
    }
}
