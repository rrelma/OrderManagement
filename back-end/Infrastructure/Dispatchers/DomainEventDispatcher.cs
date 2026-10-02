using Application.Interfaces;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dispatchers
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct = default)
        {
            foreach (var domainEvent in domainEvents)
            {
                var eventType = domainEvent.GetType();

                // Constructs generic type: IDomainEventHandler<OrderPlacedDomainEvent>
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

                // Resolves all registered observers/handlers from the DI container
                var handlers = _serviceProvider.GetServices(handlerType);

                foreach (var handler in handlers)
                {
                    if (handler is null) continue;

                    // Dynamically call HandleAsync on the observer
                    var method = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync));
                    if (method != null)
                    {
                        var task = (Task)method.Invoke(handler, new object[] { domainEvent, ct })!;
                        await task;
                    }
                }
            }
        }
    }
}
