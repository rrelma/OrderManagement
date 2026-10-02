using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderReadRepository
    {
        Task<OrderDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<OrderDetailsDto>> GetAllAsync(CancellationToken ct = default);
    }
}
