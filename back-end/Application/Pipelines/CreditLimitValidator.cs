using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pipelines
{
    public class CreditLimitValidator : AbstractOrderValidator
    {
        private const decimal MaxCreditLimit = 10000.00m;

        public override async Task ValidateAsync(CreateOrderCommand command, CancellationToken ct = default)
        {
            // Simulate credit/spending limit verification
            if (command.Amount > MaxCreditLimit)
            {
                throw new InvalidOperationException($"Validation Error: Order exceeds maximum allowed credit limit of {MaxCreditLimit:C}.");
            }

            Console.WriteLine("[VALIDATION] Credit limit check passed.");

            // Pass request to the next link in the chain
            await base.ValidateAsync(command, ct);
        }
    }
}
