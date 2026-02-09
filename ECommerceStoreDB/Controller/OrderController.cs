using ECommerceStoreDB.DTOs;
using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStoreDB.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IOrderRepository _orderRepository;
		public OrderController(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Order>> GetOrderById(int id)
		{
			var order = await _orderRepository.GetOrderByIdAsync(id);
			if (order == null)
			{
				return NotFound();
			}
			return Ok(order);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
		{
			var orders = await _orderRepository.GetAllOrdersAsync();
			if (orders == null)
			{
				return NotFound();
			}
			return Ok(orders);
		}

		[HttpPost]
		public async Task<ActionResult<Order>> InsertOrder(CreateOrderDto orderDto)
		{
			if (orderDto == null)
			{
				return BadRequest();
			}

			var order = new Order
			{
				CustomerId = orderDto.CustomerId,
				TotalAmount = orderDto.TotalAmount,
				Status = orderDto.Status
			};

			var createdOrder = await _orderRepository.InsertOrderAsync(order);
			return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderId }, createdOrder);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateOrder(int id, Order order)
		{
			if (id != order.OrderId)
			{
				return BadRequest();
			}

			var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
			if (existingOrder == null)
			{
				return NotFound();
			}

			await _orderRepository.UpdateOrderAsync(order);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
			if (existingOrder == null)
			{
				return NotFound();
			}

			await _orderRepository.DeleteOrderAsync(id);
			return NoContent();
		}
	}
}
