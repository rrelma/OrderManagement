using Application.Interfaces;
using Infrastructure.Implementation;

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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
