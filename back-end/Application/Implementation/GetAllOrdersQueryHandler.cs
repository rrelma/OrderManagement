using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementation
{
    public class GetAllOrdersQueryHandler : IGetAllOrdersQueryHandler
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public GetAllOrdersQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<IEnumerable<OrderDetailsDto>> HandleAsync(CancellationToken ct = default)
        {
            IEnumerable<OrderDetailsDto> x= await _orderReadRepository.GetAllAsync(ct);
            return x;
        }
    }
}
