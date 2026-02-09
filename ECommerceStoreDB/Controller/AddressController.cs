using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerceStoreDB.DTOs;

namespace ECommerceStoreDB.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class AddressController : ControllerBase
	{
		private readonly IAddressRepository _addressRepository;
		public AddressController(IAddressRepository addressRepository)
		{
			_addressRepository = addressRepository;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Address>> GetAddressById(int id)
		{
			var address = await _addressRepository.GetAddressByIdAsync(id);
			if (address == null)
			{
				return NotFound();
			}
			return Ok(address);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Address>>> GetAddress()
		{
			var address = await _addressRepository.GetAllAddressesAsync();
			if (address == null)
			{
				return NotFound();
			}
			return Ok(address);
		}

		[HttpPost]
		public async Task<ActionResult<Address>> InsertAddress(CreateAddressDto addressDto)
		{
			if (addressDto == null)
			{
				return BadRequest();
			}

			var address = new Address
			{
				CustomerId = addressDto.CustomerId,
				Street = addressDto.Street,
				City = addressDto.City,
				Zip = addressDto.Zip
			};

			var createdAddress = await _addressRepository.InsertAddressAsync(address);
			return CreatedAtAction(nameof(GetAddressById), new { id = createdAddress.AddressId }, createdAddress);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAddress(int id, Address address)
		{
			if (id != address.AddressId)
			{
				return BadRequest();
			}

			var existingAddress = await _addressRepository.GetAddressByIdAsync(id);
			if (existingAddress == null)
			{
				return NotFound();
			}

			await _addressRepository.UpdateAddressAsync(address);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAddress(int id)
		{
			var existingAddress = await _addressRepository.GetAddressByIdAsync(id);
			if (existingAddress == null)
			{
				return NotFound();
			}

			await _addressRepository.DeleteAddressAsync(id);
			return NoContent();
		}
	}
}
