using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementation
{
    public class ShipOrderCommandHandler : IShipOrderCommandHandler
    {
        private readonly IOrderRepository _orderRepository;

        public ShipOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(Guid orderId, CancellationToken ct = default)
        {
            var order = await _orderRepository.GetByIdAsync(orderId, ct);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} was not found.");
            }

            // Triggers State pattern transition on the Aggregate Root
            order.Ship();

            await _orderRepository.UpdateAsync(order, ct);
        }
    }
}
