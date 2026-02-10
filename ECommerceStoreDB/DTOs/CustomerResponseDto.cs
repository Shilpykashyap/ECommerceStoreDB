namespace ECommerceStoreDB.DTOs
{
	public class CustomerResponseDto
	{
		public int CustomerId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public string Phone { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
