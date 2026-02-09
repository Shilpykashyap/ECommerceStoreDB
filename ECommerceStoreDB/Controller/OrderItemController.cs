using ECommerceStoreDB.DTOs;
using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStoreDB.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderItemController : ControllerBase
	{
		private readonly IOrderItemRepository _orderItemRepository;
		public OrderItemController(IOrderItemRepository orderItemRepository)
		{
			_orderItemRepository = orderItemRepository;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<OrderItem>> GetOrderItemById(int id)
		{
			var orderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);
			if (orderItem == null)
			{
				return NotFound();
			}
			return Ok(orderItem);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
		{
			var orderItems = await _orderItemRepository.GetAllOrderItemsAsync();
			if (orderItems == null)
			{
				return NotFound();
			}
			return Ok(orderItems);
		}

		[HttpPost]
		public async Task<ActionResult<OrderItem>> InsertOrderItem(CreateOrderItemDto orderItemDto)
		{
			if (orderItemDto == null)
			{
				return BadRequest();
			}

			var orderItem = new OrderItem
			{
				OrderId = orderItemDto.OrderId,
				ProductId = orderItemDto.ProductId,
				Price = orderItemDto.Price,
				Quantity = orderItemDto.Quantity
			};

			var createdOrderItem = await _orderItemRepository.InsertOrderItemAsync(orderItem);
			return CreatedAtAction(nameof(GetOrderItemById), new { id = createdOrderItem.OrderItemsId }, createdOrderItem);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateOrderItem(int id, OrderItem orderItem)
		{
			if (id != orderItem.OrderItemsId)
			{
				return BadRequest();
			}

			var existingOrderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);
			if (existingOrderItem == null)
			{
				return NotFound();
			}

			await _orderItemRepository.UpdateOrderItemAsync(orderItem);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrderItem(int id)
		{
			var existingOrderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);
			if (existingOrderItem == null)
			{
				return NotFound();
			}

			await _orderItemRepository.DeleteOrderItemAsync(id);
			return NoContent();
		}
	}
}
