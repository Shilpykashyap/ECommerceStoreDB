using ECommerceStoreDB.Models;

namespace ECommerceStoreDB.IRepositories
{
	public interface IAuthRepository
	{
		Task<Customer> LoginAsync(string email, string password);
	}
}
