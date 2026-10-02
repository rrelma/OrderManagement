using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IShipOrderCommandHandler
    {
        Task HandleAsync(Guid orderId, CancellationToken ct = default);
    }
}
