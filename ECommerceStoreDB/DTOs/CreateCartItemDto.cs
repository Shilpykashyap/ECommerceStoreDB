namespace ECommerceStoreDB.DTOs
{
	public class CreateCartItemDto
	{
		public int CartId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
	}
}
