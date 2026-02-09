using ECommerceStoreDB.DTOs;
using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStoreDB.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartItemController : ControllerBase
	{
		private readonly ICartItemRepository _cartItemRepository;
		public CartItemController(ICartItemRepository cartItemRepository)
		{
			_cartItemRepository = cartItemRepository;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CartItem>> GetCartItemById(int id)
		{
			var cartItem = await _cartItemRepository.GetCartItemByIdAsync(id);
			if (cartItem == null)
			{
				return NotFound();
			}
			return Ok(cartItem);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
		{
			// Note: The stored procedure sp_GetAllCartItems gets ALL cart items across all carts.
			var cartItems = await _cartItemRepository.GetAllCartItemsAsync();
			if (cartItems == null)
			{
				return NotFound();
			}
			return Ok(cartItems);
		}

		[HttpPost]
		public async Task<ActionResult<CartItem>> InsertCartItem(CreateCartItemDto cartItemDto)
		{
			if (cartItemDto == null)
			{
				return BadRequest();
			}

			var cartItem = new CartItem
			{
				CartId = cartItemDto.CartId,
				ProductId = cartItemDto.ProductId,
				Quantity = cartItemDto.Quantity
			};

			var createdCartItem = await _cartItemRepository.InsertCartItemAsync(cartItem);
			return CreatedAtAction(nameof(GetCartItemById), new { id = createdCartItem.CartItemsId }, createdCartItem);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCartItem(int id, CartItem cartItem)
		{
			if (id != cartItem.CartItemsId)
			{
				return BadRequest();
			}

			var existingCartItem = await _cartItemRepository.GetCartItemByIdAsync(id);
			if (existingCartItem == null)
			{
				return NotFound();
			}

			await _cartItemRepository.UpdateCartItemAsync(cartItem);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCartItem(int id)
		{
			var existingCartItem = await _cartItemRepository.GetCartItemByIdAsync(id);
			if (existingCartItem == null)
			{
				return NotFound();
			}

			await _cartItemRepository.DeleteCartItemAsync(id);
			return NoContent();
		}
	}
}
