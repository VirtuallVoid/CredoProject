using Application.DTOs.User.Request;
using Application.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<Users>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<Users> GetUserFullInfo(UserLogin userInfo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "EXEC [dbo].[spGetUserFullInfo] @Username, @Password";
                return await conn.QueryFirstOrDefaultAsync<Users>(sql, userInfo);
            }
        }

        public async Task<int> InsertUserInfo(UserInfo userInfo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                string sql = "EXEC [dbo].[spInsertUserFullInfo] @Username, @Password, @Mobile, @FullName, @Email, @PersonalNumber, @BirthDate";
                return await conn.QueryFirstOrDefaultAsync<int>(sql, userInfo);
            }
        }
    }
}
