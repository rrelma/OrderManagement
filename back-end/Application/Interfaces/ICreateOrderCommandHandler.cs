using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public record CreateOrderCommand(
        Guid CustomerId,
        decimal Amount,
        string Currency,
        string? DiscountType,   // "Percentage", "Flat", or null
        decimal? DiscountValue, // e.g., 10 for 10%
        string PaymentProvider  // "Stripe" or "PayPal"
    );

    public interface ICreateOrderCommandHandler
    {
        Task<Guid> HandleAsync(CreateOrderCommand command, CancellationToken ct = default);
    }
}
