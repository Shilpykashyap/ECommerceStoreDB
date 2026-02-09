namespace ECommerceStoreDB.DTOs
{
	public class CreateProductDto
	{
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string? ImageUrl { get; set; }
		public bool IsActive { get; set; }
	}
}
