using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Factories;
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
        private readonly IPaymentGatewayFactory _paymentGatewayFactory;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IPaymentGatewayFactory paymentGatewayFactory)
        {
            _orderRepository = orderRepository;
            _paymentGatewayFactory = paymentGatewayFactory;
        }

        public async Task<Guid> HandleAsync(CreateOrderCommand command, CancellationToken ct = default)
        {
            // 1. Create Aggregate Root
            var order = new Order(command.CustomerId, command.Currency);
            order.AddItem(command.Amount, command.Currency);

            // 2. Apply Strategy Pattern (Discount)
            var discountStrategy = DiscountStrategyFactory.Create(command.DiscountType, command.DiscountValue);
            if (discountStrategy != null)
            {
                order.ApplyDiscount(discountStrategy);
            }

            // 3. Apply Factory Method Pattern (Payment Processing)
            var paymentGateway = _paymentGatewayFactory.GetPaymentGateway(command.PaymentProvider);

            var paymentSuccess = await paymentGateway.ProcessPaymentAsync(
                order.Id,
                order.TotalAmount.Amount,
                order.TotalAmount.Currency,
                ct);

            if (!paymentSuccess)
            {
                throw new InvalidOperationException($"Payment failed via provider: {command.PaymentProvider}");
            }

            // 4. Save to Repository
            await _orderRepository.SaveAsync(order, ct);

            return order.Id;
        }
    }
}
