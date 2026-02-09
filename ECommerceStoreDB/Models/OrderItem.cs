namespace ECommerceStoreDB.Models
{
	public class OrderItem
	{
		public int OrderItemsId { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
