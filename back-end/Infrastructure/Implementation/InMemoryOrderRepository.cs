using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Implementation
{
    public class InMemoryOrderRepository : IOrderRepository, IOrderReadRepository
    {
        private readonly List<Order> _orders = new();
        private readonly object _lock = new();

        // ------------------------------------------------------------------
        // Write Repository Operations (IOrderRepository)
        // ------------------------------------------------------------------

        public Task SaveAsync(Order order, CancellationToken ct = default)
        {
            lock (_lock)
            {
                _orders.Add(order);
            }
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Order order, CancellationToken ct = default)
        {
            lock (_lock)
            {
                var index = _orders.FindIndex(o => o.Id == order.Id);
                if (index != -1)
                {
                    _orders[index] = order;
                }
            }
            return Task.CompletedTask;
        }

        // Domain Entity Lookup for Command Handlers
        Task<Order?> IOrderRepository.GetByIdAsync(Guid id, CancellationToken ct)
        {
            lock (_lock)
            {
                var order = _orders.FirstOrDefault(o => o.Id == id);
                return Task.FromResult(order);
            }
        }

        // ------------------------------------------------------------------
        // Read Repository Operations (IOrderReadRepository)
        // ------------------------------------------------------------------

        // DTO Lookup for Query Handlers
        async Task<OrderDetailsDto?> IOrderReadRepository.GetByIdAsync(Guid id, CancellationToken ct)
        {
            lock (_lock)
            {
                var order = _orders.FirstOrDefault(o => o.Id == id);
                if (order == null) return null;

                return MapToOrderDetailsDto(order);
            }
        }

        public Task<IEnumerable<OrderDetailsDto>> GetAllAsync(CancellationToken ct = default)
        {
            lock (_lock)
            {
                var dtos = _orders.Select(MapToOrderDetailsDto).ToList();
                return Task.FromResult<IEnumerable<OrderDetailsDto>>(dtos);
            }
        }

        // ------------------------------------------------------------------
        // Private Projection Helper
        // ------------------------------------------------------------------

        private static OrderDetailsDto MapToOrderDetailsDto(Order order)
        {
            string cleanStatus = order.State.GetType().Name
                .Replace("OrderState", "")
                .Replace("State", "");

            return new OrderDetailsDto(
                order.Id,
                order.CustomerId,
                order.TotalAmount.Amount,
                order.TotalAmount.Currency,
                string.IsNullOrWhiteSpace(cleanStatus) ? "Pending" : cleanStatus,
                DateTime.UtcNow
            );
        }
    }
}