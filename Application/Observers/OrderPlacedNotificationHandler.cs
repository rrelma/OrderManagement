using Application.Interfaces;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Observers
{
    public class OrderPlacedNotificationHandler : IDomainEventHandler<OrderPlacedDomainEvent>
    {
        private readonly IEmailNotificationService _emailService;
        public OrderPlacedNotificationHandler(IEmailNotificationService emailService)
        {
            _emailService = emailService;
        }
        public async Task HandleAsync(OrderPlacedDomainEvent domainEvent, CancellationToken ct = default)
        {
            string subject = $"Order Confirmation #{domainEvent.OrderId}";
            string body = $"Thank you for your order! Total amount charged: {domainEvent.TotalAmount:C}";

            await _emailService.SendEmailAsync("customer@example.com", subject, body, ct);
        }
    }
}
