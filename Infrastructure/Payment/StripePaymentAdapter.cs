using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Payment
{
    public class StripePaymentAdapter : IPaymentGateway
    {
        public Task<bool> ProcessPaymentAsync(Guid orderId, decimal amount, string currency, CancellationToken ct = default)
        {
            // Stripe-specific API logic here
            return Task.FromResult(true);
        }
    }
}
