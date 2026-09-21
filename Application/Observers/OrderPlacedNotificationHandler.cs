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
        public Task HandleAsync(OrderPlacedDomainEvent domainEvent, CancellationToken ct = default)
        {
            Console.WriteLine($"[NOTIFICATION OBSERVER] Sending confirmation email for Order: {domainEvent.OrderId}");
            return Task.CompletedTask;
        }
    }
}
