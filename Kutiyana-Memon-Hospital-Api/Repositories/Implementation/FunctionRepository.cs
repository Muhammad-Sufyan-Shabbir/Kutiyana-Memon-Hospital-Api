using Dapper;
using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.Http;
using Kutiyana_Memon_Hospital_Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

namespace Kutiyana_Memon_Hospital_Api.Repositories.Implementation
{
    public class FunctionRepository : IFunctionRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FunctionRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Database connection string not found!");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<object> GetRoleByIdAsync(int roleId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("GetRoleById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoleId", roleId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Safe get value
                            var value = reader.GetValue(0);

                            // Agar JSON string hai to parse kar do
                            if (value is string jsonResult)
                            {
                                return JsonSerializer.Deserialize<object>(jsonResult);
                            }

                            // Warna raw value return kar do
                            return value;
                        }
                    }
                }
            }

            return new { Message = "Role not found", RoleId = roleId };
        }

        public async Task<object> GetUserByIdAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("GetUserById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Safe get value
                            var value = reader.GetValue(0);

                            // Agar JSON string hai to parse kar do
                            if (value is string jsonResult)
                            {
                                return JsonSerializer.Deserialize<object>(jsonResult);
                            }

                            // Warna raw value return kar do
                            return value;
                        }
                    }
                }
            }

            return new { Message = "User not found", UserId = userId };
        }

        public async Task<PageResponse<object>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(
                "GetAllApplicationUsers",
                new { PageNumber = pageNumber, PageSize = pageSize },
                commandType: CommandType.StoredProcedure
            );

            // 1) Total count
            var totalCount = await multi.ReadFirstAsync<int>();

            // 2) Paginated users
            var users = (await multi.ReadAsync<dynamic>()).ToList();

            return new PageResponse<object>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                RowCount = totalCount,
                PageCount = (int)Math.Ceiling((double)totalCount / pageSize),
                Rows = users
            };
        }

    }
}
