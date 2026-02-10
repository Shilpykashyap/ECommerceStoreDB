using ECommerceStoreDB.Models;

namespace ECommerceStoreDB.IRepositories
{
	public interface ICustomerRepository
	{
		Task<Customer> GetCustomerByIdAsync(int customerId);
		Task<IEnumerable<Customer>> GetAllCustomerAsync();
		Task<Customer> InsertCustomerAsync(Customer customer);
		Task UpdateCustomerAsync(Customer customer);
		Task DeleteCustomerAsync(int customerId);
	}

}
