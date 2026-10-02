using Application.Implementation;
using Application.Interfaces;
using Application.Observers;
using Application.Pipelines;
using Domain.Events;
using Infrastructure.Adapters.Notifications;
using Infrastructure.Decorators;
using Infrastructure.Dispatchers;
using Infrastructure.External;
using Infrastructure.Factories;
using Infrastructure.Implementation;
using Infrastructure.Payment;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with SQL Server connection string
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add memory cache service
builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services.AddControllers();

// Enable CORS so the Angular frontend (http://localhost:4200) can communicate with .NET
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Register Single Shared Repository Instance for In-Memory Persistence
builder.Services.AddScoped<EfOrderRepository>();
builder.Services.AddScoped<IOrderRepository>(sp => sp.GetRequiredService<EfOrderRepository>());
builder.Services.AddScoped<IOrderReadRepository>(sp => sp.GetRequiredService<EfOrderRepository>());

// Register Dispatcher
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

// Register Observers for OrderPlacedDomainEvent
builder.Services.AddScoped<IDomainEventHandler<OrderPlacedDomainEvent>, OrderPlacedNotificationHandler>();
builder.Services.AddScoped<IDomainEventHandler<OrderPlacedDomainEvent>, InventoryReservationHandler>();

// Handlers
builder.Services.AddScoped<ICreateOrderCommandHandler, CreateOrderCommandHandler>();
builder.Services.AddScoped<IGetOrderByIdQueryHandler, GetOrderByIdQueryHandler>();
builder.Services.AddScoped<IShipOrderCommandHandler, ShipOrderCommandHandler>();
builder.Services.AddScoped<IGetAllOrdersQueryHandler, GetAllOrdersQueryHandler>();


// Register Payment Adapters
builder.Services.AddScoped<StripePaymentAdapter>();
builder.Services.AddScoped<PayPalPaymentAdapter>();

// Register Factory
builder.Services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();

builder.Services.AddScoped<IEmailNotificationService, SendGridEmailAdapter>();
builder.Services.AddScoped<SendGridClientSdk>();

// 1. Register Validation Chain (Chain of Responsibility)
builder.Services.AddScoped<IOrderValidator>(sp =>
{
    var stockValidator = new StockValidator();
    var creditValidator = new CreditLimitValidator();

    // Link the chain: StockValidator -> CreditLimitValidator
    stockValidator.SetNext(creditValidator);

    return stockValidator;
});

// 2. Register Query Handler with Decorator
builder.Services.AddScoped<GetOrderByIdQueryHandler>();
builder.Services.AddScoped<IGetOrderByIdQueryHandler>(sp =>
{
    var innerHandler = sp.GetRequiredService<GetOrderByIdQueryHandler>();
    var cache = sp.GetRequiredService<IMemoryCache>();

    return new CachingOrderQueryDecorator(innerHandler, cache);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors("AllowAngularDev");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
