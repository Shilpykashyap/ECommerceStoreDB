namespace ECommerceStoreDB.Models
{
	public class Cart
	{
		public int CartId { get; set; }
		public int? CustomerId { get; set; }
		public string? SessionId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
