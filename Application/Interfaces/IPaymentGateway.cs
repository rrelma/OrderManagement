using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentGateway
    {
        Task<bool> ProcessPaymentAsync(Guid orderId, decimal amount, string currency, CancellationToken ct = default);
    }
}
