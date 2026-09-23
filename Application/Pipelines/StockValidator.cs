using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pipelines
{
    public class StockValidator : AbstractOrderValidator
    {
        public override async Task ValidateAsync(CreateOrderCommand command, CancellationToken ct = default)
        {
            // Simulate checking item availability
            if (command.Amount <= 0)
            {
                throw new InvalidOperationException("Validation Error: Order amount must be greater than zero.");
            }

            Console.WriteLine("[VALIDATION] Stock availability confirmed.");

            // Pass request to the next link in the chain
            await base.ValidateAsync(command, ct);
        }
    }
}
