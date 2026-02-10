using ECommerceStoreDB.DTOs;
using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceStoreDB.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository _authRepository;
		public AuthController(IAuthRepository authRepository)
		{
			_authRepository = authRepository;
		}

		[HttpPost("login")]
		public async Task<ActionResult<CustomerResponseDto>> Login(LoginDto loginDto)
		{
			if (loginDto == null)
			{
				return BadRequest("Invalid client request");
			}

			var customer = await _authRepository.LoginAsync(loginDto.Email, loginDto.Password);
			if (customer == null)
			{
				return Unauthorized();
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
	}
}
