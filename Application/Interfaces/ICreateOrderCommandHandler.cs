using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public record CreateOrderCommand(Guid CustomerId, decimal Amount, string Currency);

    public interface ICreateOrderCommandHandler
    {
        Task<Guid> HandleAsync(CreateOrderCommand command, CancellationToken ct = default);
    }
}
