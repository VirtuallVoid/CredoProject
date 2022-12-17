using Application.DTOs.LoanApplication.Request;
using Application.DTOs.LoanApplication.Response;
using Application.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class LoanApplicationsRepository : GenericRepository<LoanApplications>, ILoanApplicationsRepository
    {
        public LoanApplicationsRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<IEnumerable<LoanApplications>> GetUserLoanApplicationsAsync(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "EXEC [dbo].[spGetUserLoanApplications] @UserId";
                return await conn.QueryAsync<LoanApplications>(sql, new {UserId = userId});
            }
        }

        public async Task<LoanApplicationResponse> GetUserLoanApplicationByIdAsync(int userId, int loanId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "EXEC [dbo].[spGetUserLoanApplicationById] @UserId, @Id";
                return await conn.QueryFirstOrDefaultAsync<LoanApplicationResponse>(sql, new { UserId = userId, Id = loanId });
            }
        }

        public async Task<IEnumerable<StatusTypes>> GetApplicationStatusTypesAsync()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "EXEC [dbo].[spGetApplicationStatusTypes]";
                return await conn.QueryAsync<StatusTypes>(sql);
            }
        }

        public async Task<IEnumerable<LoanTypes>> GetApplicationLoanTypesAsync()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "EXEC [dbo].[spGetApplicationLoanTypes]";
                return await conn.QueryAsync<LoanTypes>(sql);
            }
        }

        public async Task<int> InsertUserLoanApplicationAsync(LoanApplicationRequest loan)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "EXEC [dbo].[spInsertUserLoanApplication] @LoanTypeId, @UserId, @Amount, @Currency, @LoanPeriod, @StatusId";
                return await conn.QueryFirstOrDefaultAsync<int>(sql, loan);
            }
        }

        public async Task<int> UpdateUserLoanApplicationAsync(LoanApplicationRequest loan) 
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "EXEC [dbo].[spUpdateUserLoanApplication] @Id, @LoanTypeId, @UserId, @Amount, @Currency, @LoanPeriod, @StatusId";
                var result = await conn.QueryFirstOrDefaultAsync<int>(sql, loan);
                return result;
            }
        }

        public async Task<int> DeleteUserLoanApplicationAsync(int userId, int loanId) 
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "EXEC [dbo].[spDeleteUserLoanApplication] @LoanId, @UserId";
                return await conn.QueryFirstOrDefaultAsync<int>(sql, new { LoanId = loanId, UserId = userId });
            }
        }
    }
}
