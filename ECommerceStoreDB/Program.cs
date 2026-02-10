using ECommerceStoreDB.IRepositories;
using ECommerceStoreDB.Repositories;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

//builder.Services.AddScoped<ICustomerRepository>(provider =>
//{
//	var configuration = provider.GetRequiredService<IConfiguration>();
//	var connectionString = configuration.GetConnectionString("StoreFrontDb");
//	if (string.IsNullOrEmpty(connectionString))
//	{
//		throw new InvalidOperationException("Connection string 'StoreFrontDb' is not configured.");
//	}
//	return new CustomerRepository(connectionString);
//});

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); 

app.Run();