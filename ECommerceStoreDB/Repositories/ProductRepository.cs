using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerceStoreDB.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly string _connectionString;
		public ProductRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("StoreFrontDb");
		}

		public async Task<Product> GetProductByIdAsync(int productId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetProductById", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@ProductId", productId);
					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							return new Product
							{
								ProductId = reader.GetInt32(0),
								Name = reader.GetString(1),
								Price = reader.GetDecimal(2),
								ImageUrl = reader.IsDBNull(3) ? null : reader.GetString(3),
								IsActive = reader.GetBoolean(4)
							};
						}
					}
				}
			}
			return null;
		}

		public async Task<IEnumerable<Product>> GetAllProductsAsync()
		{
			var products = new List<Product>();
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_GetAllProducts", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							products.Add(new Product
							{
								ProductId = reader.GetInt32(0),
								Name = reader.GetString(1),
								Price = reader.GetDecimal(2),
								ImageUrl = reader.IsDBNull(3) ? null : reader.GetString(3),
								IsActive = reader.GetBoolean(4)
							});
						}
					}
				}
			}
			return products;
		}

		public async Task<Product> InsertProductAsync(Product product)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_InsertProduct", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@Name", product.Name);
					command.Parameters.AddWithValue("@Price", product.Price);
					command.Parameters.AddWithValue("@ImageUrl", (object)product.ImageUrl ?? DBNull.Value);
					command.Parameters.AddWithValue("@IsActive", product.IsActive);

					product.ProductId = Convert.ToInt32(await command.ExecuteScalarAsync());
					return product;
				}
			}
		}

		public async Task UpdateProductAsync(Product product)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_UpdateProduct", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@ProductId", product.ProductId);
					command.Parameters.AddWithValue("@Name", product.Name);
					command.Parameters.AddWithValue("@Price", product.Price);
					command.Parameters.AddWithValue("@ImageUrl", (object)product.ImageUrl ?? DBNull.Value);
					command.Parameters.AddWithValue("@IsActive", product.IsActive);

					await command.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task DeleteProductAsync(int productId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var command = new SqlCommand("sp_DeleteProduct", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@ProductId", productId);

					await command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
