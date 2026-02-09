using ECommerceStoreDB.DTOs;
using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStoreDB.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerController : ControllerBase
	{
		private readonly ICustomerRepository _customerRepository;
		public CustomerController(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Customer>> GetCustomerById(int id)
		{
			var customer = await _customerRepository.GetCustomerByIdAsync(id);
			if(customer == null)
			{
				return NotFound();
			}
			return Ok(customer);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
		{
			var customer = await _customerRepository.GetAllCustomerAsync();
			if(customer == null)
			{
				return NotFound();
			}
			return Ok(customer);
		}

		[HttpPost]
		public async Task<ActionResult<Customer>> InsertCustomer(CreateCustomerDto customerDto)
		{
			if (customerDto == null)
			{
				return BadRequest();
			}

			var customer = new Customer
			{
				Name = customerDto.Name,
				Email = customerDto.Email,
				Password = customerDto.Password,
				Role = customerDto.Role,
				Phone = customerDto.Phone
			};

			var createdCustomer = await _customerRepository.InsertCustomerAsync(customer);
			return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.CustomerId }, createdCustomer);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
		{
			if (id != customer.CustomerId)
			{
				return BadRequest();
			}

			var existingCustomer = await _customerRepository.GetCustomerByIdAsync(id);
			if (existingCustomer == null)
			{
				return NotFound();
			}

			await _customerRepository.UpdateCustomerAsync(customer);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCustomer(int id)
		{
			var existingCustomer = await _customerRepository.GetCustomerByIdAsync(id);
			if (existingCustomer == null)
			{
				return NotFound();
			}

			await _customerRepository.DeleteCustomerAsync(id);
			return NoContent();
		}
	}
}
