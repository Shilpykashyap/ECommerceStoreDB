using ECommerceStoreDB.Models;

namespace ECommerceStoreDB.IRepositories
{
	public interface IAddressRepository
	{
		Task<Address> GetAddressByIdAsync(int addressId);
		Task<IEnumerable<Address>> GetAllAddressesAsync();
		Task<Address> InsertAddressAsync(Address address);
		Task UpdateAddressAsync(Address address);
		Task DeleteAddressAsync(int addressId);
	}
}
