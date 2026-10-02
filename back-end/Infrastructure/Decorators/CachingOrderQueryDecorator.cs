using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Decorators
{
    public class CachingOrderQueryDecorator : IGetOrderByIdQueryHandler
    {
        private readonly IGetOrderByIdQueryHandler _innerHandler;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public CachingOrderQueryDecorator(IGetOrderByIdQueryHandler innerHandler, IMemoryCache cache)
        {
            _innerHandler = innerHandler;
            _cache = cache;
        }

        public async Task<OrderDetailsDto?> HandleAsync(Guid orderId, CancellationToken ct = default)
        {
            string cacheKey = $"order-{orderId}";

            if (_cache.TryGetValue(cacheKey, out OrderDetailsDto? cachedOrder))
            {
                Console.WriteLine($"[CACHE HIT] Retrieved Order {orderId} from memory cache.");
                return cachedOrder;
            }

            Console.WriteLine($"[CACHE MISS] Order {orderId} not found in cache. Delegating to inner query handler...");
            var order = await _innerHandler.HandleAsync(orderId, ct);

            if (order != null)
            {
                _cache.Set(cacheKey, order, CacheDuration);
            }

            return order;
        }
    }
}
