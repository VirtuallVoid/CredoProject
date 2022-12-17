using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using System.Reflection;
using Domain.Common;

namespace Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly string _tableName;
        protected string _connectionString;

        public GenericRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbConnection");
            _tableName = $"TestProjectDB.dbo.{typeof(T).Name}";
        }

        public async Task<int> AddAsync(T entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
            var query = $"INSERT INTO {_tableName} ({stringOfColumns}) VALUES ({stringOfParameters}) SELECT SCOPE_IDENTITY()";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var result = await conn.QueryFirstOrDefaultAsync<int>(query, entity);
                return result;
            }
        }
        public async Task UpdateAsync(T entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
            var query = $"UPDATE {_tableName} set {stringOfColumns} WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                await conn.ExecuteAsync(query, entity);
            }
        }
        public async Task DeleteAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                await conn.ExecuteAsync($"DELETE FROM {_tableName} WHERE [Id] = @Id", new { Id = id });
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var data = await conn.QueryAsync<T>($"SELECT * FROM {_tableName}");
                return data;
            }
        }
        public async Task<T> GetByIdAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var data = await conn.QueryAsync<T>($"SELECT * FROM {_tableName} WHERE Id = @Id", new { Id = id });
                return data.FirstOrDefault();
            }

        }
        private IEnumerable<string> GetColumns()
        {
            return typeof(T)
                    .GetProperties()
                    .Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Select(e => e.Name);
        }
    }
}
