using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementation
{
    public class GetOrderByIdQueryHandler : IGetOrderByIdQueryHandler
    {
        private readonly IOrderReadRepository _readRepository;

        public GetOrderByIdQueryHandler(IOrderReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<OrderDetailsDto?> HandleAsync(Guid orderId, CancellationToken ct = default)
        {
            Console.WriteLine($"[DATABASE] Fetching Order {orderId} from persistence store...");
            return await _readRepository.GetByIdAsync(orderId, ct);
        }
    }
}
