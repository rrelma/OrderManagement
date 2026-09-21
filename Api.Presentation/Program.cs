using Application.Interfaces;
using Application.Observers;
using Domain.Events;
using Infrastructure.Dispatchers;
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

// Register Dispatcher
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

// Register Observers for OrderPlacedDomainEvent
builder.Services.AddScoped<IDomainEventHandler<OrderPlacedDomainEvent>, OrderPlacedNotificationHandler>();
builder.Services.AddScoped<IDomainEventHandler<OrderPlacedDomainEvent>, InventoryReservationHandler>();

// Handlers
builder.Services.AddScoped<ICreateOrderCommandHandler, CreateOrderCommandHandler>();
builder.Services.AddScoped<IGetOrderByIdQueryHandler, GetOrderByIdQueryHandler>();

// Register Payment Adapters
builder.Services.AddScoped<StripePaymentAdapter>();
builder.Services.AddScoped<PayPalPaymentAdapter>();

// Register Factory
builder.Services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
