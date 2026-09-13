using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public record GetOrderByIdQuery(Guid OrderId);

    public interface IGetOrderByIdQueryHandler
    {
        Task<OrderDetailsDto?> HandleAsync(GetOrderByIdQuery query, CancellationToken ct = default);
    }
}
