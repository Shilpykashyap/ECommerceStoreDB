using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;


namespace ECommerceStoreDB.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly string _connectionString;
		public CustomerRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("StoreFrontDb");
		}

		public async Task<Customer> GetCustomerByIdAsync(int customerId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetCustomerById", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CustomerId", customerId);
					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							return new Customer
							{
								CustomerId = reader.GetInt32(0),
								Name = reader.GetString(1),
								Email = reader.GetString(2),
								Password = reader.GetString(3),
								Role = reader.GetString(4),
								Phone = reader.IsDBNull(5) ? null : reader.GetString(5),
								CreatedAt = reader.GetDateTime(6),
							};
						}
					}
				}
			}
			return null;
		}

		public async Task<IEnumerable<Customer>> GetAllCustomerAsync()
		{
			var customer = new List<Customer>();
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetAllCustomers", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							customer.Add(new Customer
							{
								CustomerId = reader.GetInt32(0),
								Name = reader.GetString(1),
								Email = reader.GetString(2),
								Password = reader.GetString(3),
								Role = reader.GetString(4),
								Phone = reader.IsDBNull(5) ? null : reader.GetString(5),
								CreatedAt = reader.GetDateTime(6),
							});
						}
					}
				}
			}
			return customer;
		}

		public async Task<Customer> InsertCustomerAsync(Customer customer)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_InsertCustomer", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@Name", customer.Name);
					command.Parameters.AddWithValue("@Email", customer.Email);
					command.Parameters.AddWithValue("@Password", customer.Password);
					command.Parameters.AddWithValue("@Role", customer.Role);
					command.Parameters.AddWithValue("@Phone", (object)customer.Phone ?? DBNull.Value);

				
					customer.CustomerId = Convert.ToInt32(await command.ExecuteScalarAsync());
					return customer;
				}
			}
		}

		public async Task UpdateCustomerAsync(Customer customer)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_UpdateCustomer", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
					command.Parameters.AddWithValue("@Name", customer.Name);
					command.Parameters.AddWithValue("@Email", customer.Email);
					command.Parameters.AddWithValue("@Password", customer.Password);
					command.Parameters.AddWithValue("@Role", customer.Role);
					command.Parameters.AddWithValue("@Phone", (object)customer.Phone ?? DBNull.Value);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task DeleteCustomerAsync(int customerId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_DeleteCustomer", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CustomerId", customerId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}

}


