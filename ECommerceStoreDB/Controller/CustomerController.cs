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
		public async Task<ActionResult<CustomerResponseDto>> GetCustomerById(int id)
		{
			var customer = await _customerRepository.GetCustomerByIdAsync(id);
			if(customer == null)
			{
				return NotFound();
			}
			
			var response = new CustomerResponseDto
			{
				CustomerId = customer.CustomerId,
				Name = customer.Name,
				Email = customer.Email,
				Role = customer.Role,
				Phone = customer.Phone,
				CreatedAt = customer.CreatedAt
			};

			return Ok(response);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomer()
		{
			var customers = await _customerRepository.GetAllCustomerAsync();
			if(customers == null)
			{
				return NotFound();
			}

			var response = customers.Select(c => new CustomerResponseDto
			{
				CustomerId = c.CustomerId,
				Name = c.Name,
				Email = c.Email,
				Role = c.Role,
				Phone = c.Phone,
				CreatedAt = c.CreatedAt
			});

			return Ok(response);
		}

		[HttpPost]
		public async Task<ActionResult<CustomerResponseDto>> InsertCustomer(CreateCustomerDto customerDto)
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
			
			var response = new CustomerResponseDto
			{
				CustomerId = createdCustomer.CustomerId,
				Name = createdCustomer.Name,
				Email = createdCustomer.Email,
				Role = createdCustomer.Role,
				Phone = createdCustomer.Phone,
				CreatedAt = createdCustomer.CreatedAt
			};

			return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.CustomerId }, response);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDto customerDto)
		{
			if (customerDto == null)
			{
				return BadRequest("Invalid client request");
			}

			var existingCustomer = await _customerRepository.GetCustomerByIdAsync(id);
			if (existingCustomer == null)
			{
				return NotFound();
			}

			// Update only allowed fields
			existingCustomer.Name = customerDto.Name;
			existingCustomer.Email = customerDto.Email;
			existingCustomer.Phone = customerDto.Phone;

			await _customerRepository.UpdateCustomerAsync(existingCustomer);
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
