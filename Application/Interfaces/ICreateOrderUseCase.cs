using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public record CreateOrderCommand(Guid CustomerId, decimal Amount, string Currency);

    public interface ICreateOrderUseCase
    {
        Task<Guid> ExecuteAsync(CreateOrderCommand command, CancellationToken ct = default);
    }
}
