using Kutiyana_Memon_Hospital_Api.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
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

    }
}
