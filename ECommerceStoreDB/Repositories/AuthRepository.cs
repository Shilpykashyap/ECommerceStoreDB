using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerceStoreDB.Repositories
{
	public class AuthRepository : IAuthRepository
	{
		private readonly string _connectionString;
		public AuthRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("StoreFrontDb");
		}

		public async Task<Customer> LoginAsync(string email, string password)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
			
				string query = "SELECT CustomerId, Name, Email, Role, Phone, CreatedAt FROM Customers WHERE Email = @Email AND Password = @Password";
				
				using (var command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Email", email);
					command.Parameters.AddWithValue("@Password", password);

					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							return new Customer
							{
								CustomerId = reader.GetInt32(0),
								Name = reader.GetString(1),
								Email = reader.GetString(2),
								// Password is NOT selected, nor returned
								Role = reader.GetString(3),
								Phone = reader.IsDBNull(4) ? null : reader.GetString(4),
								CreatedAt = reader.GetDateTime(5),
							};
						}
					}
				}
			}
			return null;
		}
	}
}
