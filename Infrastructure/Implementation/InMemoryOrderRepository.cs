using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Application.DTOs;

namespace Infrastructure.Implementation
{
    public class InMemoryOrderRepository : IOrderRepository, IOrderReadRepository
    {
        private readonly List<Order> _orders = new();

        public Task SaveAsync(Order order, CancellationToken ct = default)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }

        public Task<OrderDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return Task.FromResult<OrderDetailsDto?>(null);

            var dto = new OrderDetailsDto(
                order.Id,
                order.CustomerId,
                order.TotalAmount.Amount,
                order.TotalAmount.Currency,
                order.State.ToString(),
                DateTime.UtcNow
            );

            return Task.FromResult<OrderDetailsDto?>(dto);
        }
    }
}
