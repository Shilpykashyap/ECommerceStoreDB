using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerceStoreDB.DTOs;

namespace ECommerceStoreDB.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly ICartRepository _cartRepository;
		public CartController(ICartRepository cartRepository)
		{
			_cartRepository = cartRepository;
			//cart
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Cart>> GetCartById(int id)
		{
			var cart = await _cartRepository.GetCartByIdAsync(id);
			if (cart == null)
			{
				return NotFound();
			}
			return Ok(cart);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
		{
			var carts = await _cartRepository.GetAllCartsAsync();
			return Ok(carts);
		}

		[HttpPost]
		public async Task<ActionResult<Cart>> InsertCart(CreateCartDto cartDto)
		{
			if (cartDto == null)
			{
				return BadRequest();
			}

			var cart = new Cart
			{
				CustomerId = cartDto.CustomerId,
				SessionId = cartDto.SessionId
			};

			var createdCart = await _cartRepository.InsertCartAsync(cart);
			return CreatedAtAction(nameof(GetCartById), new { id = createdCart.CartId }, createdCart);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCart(int id, Cart cart)
		{
			if (id != cart.CartId)
			{
				return BadRequest();
			}

			var existingCart = await _cartRepository.GetCartByIdAsync(id);
			if (existingCart == null)
			{
				return NotFound();
			}

			await _cartRepository.UpdateCartAsync(cart);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCart(int id)
		{
			var existingCart = await _cartRepository.GetCartByIdAsync(id);
			if (existingCart == null)
			{
				return NotFound();
			}

			await _cartRepository.DeleteCartAsync(id);
			return NoContent();
		}
	}
}
