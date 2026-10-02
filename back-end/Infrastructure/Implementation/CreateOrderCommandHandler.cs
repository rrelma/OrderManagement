using Application.Interfaces;
using Application.Pipelines;
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
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IOrderValidator _validatorPipeline;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IPaymentGatewayFactory paymentGatewayFactory,
            IDomainEventDispatcher domainEventDispatcher,
            IOrderValidator validatorPipeline)
        {
            _orderRepository = orderRepository;
            _paymentGatewayFactory = paymentGatewayFactory;
            _domainEventDispatcher = domainEventDispatcher;
            _validatorPipeline = validatorPipeline;
        }

        public async Task<Guid> HandleAsync(CreateOrderCommand command, CancellationToken ct = default)
        {
            await _validatorPipeline.ValidateAsync(command, ct);
         
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

            // State Pattern Transition
            order.MarkAsPaid();

            // 4. Save to Repository
            await _orderRepository.SaveAsync(order, ct);

            // Observer Pattern: Dispatch Domain Events to Observers
            await _domainEventDispatcher.DispatchAsync(order.DomainEvents, ct);
            order.ClearDomainEvents();

            return order.Id;
        }
    }
}
