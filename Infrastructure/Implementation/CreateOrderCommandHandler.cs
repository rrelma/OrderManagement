using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation
{
    public class CreateOrderCommandHandler : ICreateOrderCommandHandler
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Guid> HandleAsync(CreateOrderCommand command, CancellationToken ct = default)
        {
            var order = new Order(command.CustomerId, command.Currency);
            order.AddItem(command.Amount, command.Currency);
            await _orderRepository.SaveAsync(order, ct);
            return order.Id;
        }
    }
}
