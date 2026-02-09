using ECommerceStoreDB.Models;

namespace ECommerceStoreDB.IRepositories
{
	public interface ICartItemRepository
	{
		Task<CartItem> GetCartItemByIdAsync(int cartItemsId);
		Task<IEnumerable<CartItem>> GetAllCartItemsAsync();
		Task<CartItem> InsertCartItemAsync(CartItem cartItem);
		Task UpdateCartItemAsync(CartItem cartItem);
		Task DeleteCartItemAsync(int cartItemsId);
	}
}
