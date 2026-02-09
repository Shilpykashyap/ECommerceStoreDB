using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerceStoreDB.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly string _connectionString;
		public OrderRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("StoreFrontDb");
		}

		public async Task<Order> GetOrderByIdAsync(int orderId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetOrderById", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@OrderId", orderId);
					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							return new Order
							{
								OrderId = reader.GetInt32(0),
								CustomerId = reader.GetInt32(1),
								TotalAmount = reader.GetDecimal(2),
								Status = reader.GetString(3),
								OrderDate = reader.GetDateTime(4)
							};
						}
					}
				}
			}
			return null;
		}

		public async Task<IEnumerable<Order>> GetAllOrdersAsync()
		{
			var orders = new List<Order>();
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetAllOrders", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							orders.Add(new Order
							{
								OrderId = reader.GetInt32(0),
								CustomerId = reader.GetInt32(1),
								TotalAmount = reader.GetDecimal(2),
								Status = reader.GetString(3),
								OrderDate = reader.GetDateTime(4)
							});
						}
					}
				}
			}
			return orders;
		}

		public async Task<Order> InsertOrderAsync(Order order)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_InsertOrder", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@CustomerId", order.CustomerId);
					command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
					// Default to 'Pending' if nu
					//
					// ll, though SQL default handles this too if param is omitted.
					// Since we are passing a param, we should pass a value or DBNull if allowed, but Status is not nullable in model.
					// Assuming user sends a status or we default it here. User SQL has default 'Pending'.
					command.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(order.Status) ? "Pending" : order.Status);

					order.OrderId = Convert.ToInt32(await command.ExecuteScalarAsync());
					return order;
				}
			}
		}

		public async Task UpdateOrderAsync(Order order)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_UpdateOrder", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@OrderId", order.OrderId);
					command.Parameters.AddWithValue("@CustomerId", order.CustomerId);
					command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
					command.Parameters.AddWithValue("@Status", order.Status);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task DeleteOrderAsync(int orderId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_DeleteOrder", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@OrderId", orderId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
