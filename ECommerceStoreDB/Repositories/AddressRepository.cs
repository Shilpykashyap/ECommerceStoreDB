using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerceStoreDB.Repositories
{
	public class AddressRepository : IAddressRepository
	{
		private readonly string _connectionString;
		public AddressRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("StoreFrontDb");
		}

		public async Task<Address> GetAddressByIdAsync(int addressId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetAddressById", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@AddressId", addressId);
					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							return new Address
							{
								AddressId = reader.GetInt32(0),
								CustomerId = reader.GetInt32(1),
								Street = reader.GetString(2),
								City = reader.GetString(3),
								Zip = reader.GetString(4)
							};
						}
					}
				}
			}
			return null;
		}

		public async Task<IEnumerable<Address>> GetAllAddressesAsync()
		{
			var addresses = new List<Address>();
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				// NOTE: sp_GetAddress does not return AddressId based on user snippet.
				// This might cause issues if AddressId is needed for updates/deletes.
				// Implementing as requested.
				using (var command = new SqlCommand("sp_GetAddress", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							addresses.Add(new Address
							{
								// AddressId is not returned by sp_GetAddress
								CustomerId = reader.GetInt32(0),
								Street = reader.GetString(1),
								City = reader.GetString(2),
								Zip = reader.GetString(3)
							});
						}
					}
				}
			}
			return addresses;
		}

		public async Task<Address> InsertAddressAsync(Address address)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_InsertAddress", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CustomerId", address.CustomerId);
					command.Parameters.AddWithValue("@Street", address.Street);
					command.Parameters.AddWithValue("@City", address.City);
					command.Parameters.AddWithValue("@Zip", address.Zip);

					address.AddressId = Convert.ToInt32(await command.ExecuteScalarAsync());
					return address;
				}
			}
		}

		public async Task UpdateAddressAsync(Address address)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_UpdateAddress", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@AddressId", address.AddressId);
					command.Parameters.AddWithValue("@CustomerId", address.CustomerId);
					command.Parameters.AddWithValue("@Street", address.Street);
					command.Parameters.AddWithValue("@City", address.City);
					command.Parameters.AddWithValue("@Zip", address.Zip);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task DeleteAddressAsync(int addressId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_DeleteAddress", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@AddressId", addressId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
