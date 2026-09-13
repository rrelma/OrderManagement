using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation
{
    public class GetOrderByIdQueryHandler : IGetOrderByIdQueryHandler
    {
        private readonly IOrderReadRepository _readRepository; // Or direct Dapper / DbContext

        public GetOrderByIdQueryHandler(IOrderReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<OrderDetailsDto?> HandleAsync(GetOrderByIdQuery query, CancellationToken ct = default)
        {
            return await _readRepository.GetByIdAsync(query.OrderId, ct);
        }
    }
}
