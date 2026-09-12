using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public Task SaveAsync(Order order, CancellationToken ct = default)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }
    }
}
