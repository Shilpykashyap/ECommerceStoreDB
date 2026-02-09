using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerceStoreDB.Repositories
{
	public class OrderItemRepository : IOrderItemRepository
	{
		private readonly string _connectionString;
		public OrderItemRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("StoreFrontDb");
		}

		public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemsId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetOrderItemById", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@OrderItemsId", orderItemsId);
					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							return new OrderItem
							{
								OrderItemsId = reader.GetInt32(0),
								OrderId = reader.GetInt32(1),
								ProductId = reader.GetInt32(2),
								Price = reader.GetDecimal(3),
								Quantity = reader.GetInt32(4)
							};
						}
					}
				}
			}
			return null;
		}

		public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
		{
			var orderItems = new List<OrderItem>();
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetAllOrderItems", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							orderItems.Add(new OrderItem
							{
								OrderItemsId = reader.GetInt32(0),
								OrderId = reader.GetInt32(1),
								ProductId = reader.GetInt32(2),
								Price = reader.GetDecimal(3),
								Quantity = reader.GetInt32(4)
							});
						}
					}
				}
			}
			return orderItems;
		}

		public async Task<OrderItem> InsertOrderItemAsync(OrderItem orderItem)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_InsertOrderItem", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@OrderId", orderItem.OrderId);
					command.Parameters.AddWithValue("@ProductId", orderItem.ProductId);
					command.Parameters.AddWithValue("@Price", orderItem.Price);
					command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);

					orderItem.OrderItemsId = Convert.ToInt32(await command.ExecuteScalarAsync());
					return orderItem;
				}
			}
		}

		public async Task UpdateOrderItemAsync(OrderItem orderItem)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_UpdateOrderItem", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@OrderItemsId", orderItem.OrderItemsId);
					command.Parameters.AddWithValue("@OrderId", orderItem.OrderId);
					command.Parameters.AddWithValue("@ProductId", orderItem.ProductId);
					command.Parameters.AddWithValue("@Price", orderItem.Price);
					command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task DeleteOrderItemAsync(int orderItemsId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_DeleteOrderItem", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@OrderItemsId", orderItemsId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
