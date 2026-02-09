using ECommerceStoreDB.Models;

namespace ECommerceStoreDB.IRepositories
{
	public interface ICartRepository
	{
		Task<Cart> GetCartByIdAsync(int cartId);
		Task<IEnumerable<Cart>> GetAllCartsAsync();
		Task<Cart> InsertCartAsync(Cart cart);
		Task UpdateCartAsync(Cart cart);
		Task DeleteCartAsync(int cartId);
	}
}
