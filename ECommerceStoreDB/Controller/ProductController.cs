using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerceStoreDB.DTOs;

namespace ECommerceStoreDB.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductRepository _productRepository;
		public ProductController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProductById(int id)
		{
			var product = await _productRepository.GetProductByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _productRepository.GetAllProductsAsync();
			return Ok(products);
		}

		[HttpPost]
		public async Task<ActionResult<Product>> InsertProduct(CreateProductDto productDto)
		{
			if (productDto == null)
			{
				return BadRequest();
			}

			var product = new Product
			{
				Name = productDto.Name,
				Price = productDto.Price,
				ImageUrl = productDto.ImageUrl,
				IsActive = productDto.IsActive
			};

			var createdProduct = await _productRepository.InsertProductAsync(product);
			return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, createdProduct);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProduct(int id, Product product)
		{
			if (id != product.ProductId)
			{
				return BadRequest();
			}

			var existingProduct = await _productRepository.GetProductByIdAsync(id);
			if (existingProduct == null)
			{
				return NotFound();
			}

			await _productRepository.UpdateProductAsync(product);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var existingProduct = await _productRepository.GetProductByIdAsync(id);
			if (existingProduct == null)
			{
				return NotFound();
			}

			await _productRepository.DeleteProductAsync(id);
			return NoContent();
		}
	}
}
