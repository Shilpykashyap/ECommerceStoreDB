using ECommerceStoreDB.Models;

namespace ECommerceStoreDB.IRepositories
{
	public interface IOrderItemRepository
	{
		Task<OrderItem> GetOrderItemByIdAsync(int orderItemsId);
		Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
		Task<OrderItem> InsertOrderItemAsync(OrderItem orderItem);
		Task UpdateOrderItemAsync(OrderItem orderItem);
		Task DeleteOrderItemAsync(int orderItemsId);
	}
}
