using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerceStoreDB.Repositories
{
	public class CartItemRepository : ICartItemRepository
	{
		private readonly string _connectionString;
		public CartItemRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("StoreFrontDb");
		}

		public async Task<CartItem> GetCartItemByIdAsync(int cartItemsId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetCartItemById", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CartItemsId", cartItemsId);
					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							return new CartItem
							{
								CartItemsId = reader.GetInt32(0),
								CartId = reader.GetInt32(1),
								ProductId = reader.GetInt32(2),
								Quantity = reader.GetInt32(3)
							};
						}
					}
				}
			}
			return null;
		}

		public async Task<IEnumerable<CartItem>> GetAllCartItemsAsync()
		{
			var cartItems = new List<CartItem>();
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetAllCartItems", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							cartItems.Add(new CartItem
							{
								CartItemsId = reader.GetInt32(0),
								CartId = reader.GetInt32(1),
								ProductId = reader.GetInt32(2),
								Quantity = reader.GetInt32(3)
							});
						}
					}
				}
			}
			return cartItems;
		}

		public async Task<CartItem> InsertCartItemAsync(CartItem cartItem)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_InsertCartItem", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CartId", cartItem.CartId);
					command.Parameters.AddWithValue("@ProductId", cartItem.ProductId);
					command.Parameters.AddWithValue("@Quantity", cartItem.Quantity);

					cartItem.CartItemsId = Convert.ToInt32(await command.ExecuteScalarAsync());
					return cartItem;
				}
			}
		}

		public async Task UpdateCartItemAsync(CartItem cartItem)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_UpdateCartItem", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CartItemsId", cartItem.CartItemsId);
					command.Parameters.AddWithValue("@CartId", cartItem.CartId);
					command.Parameters.AddWithValue("@ProductId", cartItem.ProductId);
					command.Parameters.AddWithValue("@Quantity", cartItem.Quantity);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task DeleteCartItemAsync(int cartItemsId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_DeleteCartItem", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CartItemsId", cartItemsId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
