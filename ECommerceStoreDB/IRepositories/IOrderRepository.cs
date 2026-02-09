using ECommerceStoreDB.Models;

namespace ECommerceStoreDB.IRepositories
{
	public interface IOrderRepository
	{
		Task<Order> GetOrderByIdAsync(int orderId);
		Task<IEnumerable<Order>> GetAllOrdersAsync();
		Task<Order> InsertOrderAsync(Order order);
		Task UpdateOrderAsync(Order order);
		Task DeleteOrderAsync(int orderId);
	}
}
