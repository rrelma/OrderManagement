using Application.Interfaces;
using Infrastructure.Factories;
using Infrastructure.Implementation;
using Infrastructure.Payment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register Single Shared Repository Instance for In-Memory Persistence
var inMemoryRepo = new InMemoryOrderRepository();
builder.Services.AddSingleton<IOrderRepository>(inMemoryRepo);
builder.Services.AddSingleton<IOrderReadRepository>(inMemoryRepo);


// Handlers
builder.Services.AddScoped<ICreateOrderCommandHandler, CreateOrderCommandHandler>();
builder.Services.AddScoped<IGetOrderByIdQueryHandler, GetOrderByIdQueryHandler>();

// Register Payment Adapters
builder.Services.AddScoped<StripePaymentAdapter>();
builder.Services.AddScoped<PayPalPaymentAdapter>();

// Register Factory
builder.Services.AddSingleton<IPaymentGatewayFactory, PaymentGatewayFactory>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
