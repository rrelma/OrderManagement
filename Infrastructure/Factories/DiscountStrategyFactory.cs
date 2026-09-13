using Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Factories
{
    public static class DiscountStrategyFactory
    {
        public static IDiscountStrategy? Create(string? type, decimal? value)
        {
            if (string.IsNullOrWhiteSpace(type) || !value.HasValue)
                return null;

            return type.ToLowerInvariant() switch
            {
                "percentage" => new PercentageDiscountStrategy(value.Value),
                "flat" => new FlatDiscountStrategy(value.Value),
                _ => throw new ArgumentException($"Invalid discount type: {type}")
            };
        }
    }
}
