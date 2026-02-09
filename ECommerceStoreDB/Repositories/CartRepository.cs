using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerceStoreDB.Repositories
{
	public class CartRepository : ICartRepository
	{
		private readonly string _connectionString;
		public CartRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("StoreFrontDb");
		}

		public async Task<Cart> GetCartByIdAsync(int cartId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetCartById", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CartId", cartId);
					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							return new Cart
							{
								CartId = reader.GetInt32(0),
								CustomerId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
								SessionId = reader.IsDBNull(2) ? null : reader.GetString(2),
								CreatedAt = reader.GetDateTime(3)
							};
						}
					}
				}
			}
			return null;
		}

		public async Task<IEnumerable<Cart>> GetAllCartsAsync()
		{
			var carts = new List<Cart>();
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetAllCarts", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							carts.Add(new Cart
							{
								CartId = reader.GetInt32(0),
								CustomerId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
								SessionId = reader.IsDBNull(2) ? null : reader.GetString(2),
								CreatedAt = reader.GetDateTime(3)
							});
						}
					}
				}
			}
			return carts;
		}

		public async Task<Cart> InsertCartAsync(Cart cart)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_InsertCart", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CustomerId", (object)cart.CustomerId ?? DBNull.Value);
					command.Parameters.AddWithValue("@SessionId", (object)cart.SessionId ?? DBNull.Value);

					cart.CartId = Convert.ToInt32(await command.ExecuteScalarAsync());
					return cart;
				}
			}
		}

		public async Task UpdateCartAsync(Cart cart)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_UpdateCart", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CartId", cart.CartId);
					command.Parameters.AddWithValue("@CustomerId", (object)cart.CustomerId ?? DBNull.Value);
					command.Parameters.AddWithValue("@SessionId", (object)cart.SessionId ?? DBNull.Value);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task DeleteCartAsync(int cartId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_DeleteCart", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CartId", cartId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
