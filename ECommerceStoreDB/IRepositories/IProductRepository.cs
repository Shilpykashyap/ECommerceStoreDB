using ECommerceStoreDB.Models;

namespace ECommerceStoreDB.IRepositories
{
	public interface IProductRepository
	{
		Task<Product> GetProductByIdAsync(int productId);
		Task<IEnumerable<Product>> GetAllProductsAsync();
		Task<Product> InsertProductAsync(Product product);
		Task UpdateProductAsync(Product product);
		Task DeleteProductAsync(int productId);
	}
}
