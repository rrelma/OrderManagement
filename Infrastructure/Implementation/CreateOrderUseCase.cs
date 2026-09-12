using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation
{
    public class CreateOrderUseCase : ICreateOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Guid> ExecuteAsync(CreateOrderCommand command, CancellationToken ct = default)
        {
            // 1. Create rich domain entity (runs domain validation inside constructor/methods)
            var order = new Order(command.CustomerId, command.Currency);
            order.AddItem(command.Amount, command.Currency);

            // 2. Save entity via the secondary port
            await _orderRepository.SaveAsync(order, ct);

            // 3. Return created Order ID
            return order.Id;
        }
    }
}
