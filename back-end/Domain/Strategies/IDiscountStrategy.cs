using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Strategies
{
    public interface IDiscountStrategy
    {
        decimal ApplyDiscount(decimal originalAmount);
    }

    // Concrete Strategy 1: Percentage
    public class PercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly decimal _percentage;

        public PercentageDiscountStrategy(decimal percentage)
        {
            if (percentage is < 0 or > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 100.");
            _percentage = percentage;
        }

        public decimal ApplyDiscount(decimal originalAmount) => originalAmount * (1 - (_percentage / 100m));
    }

    // Concrete Strategy 2: Flat Amount
    public class FlatDiscountStrategy : IDiscountStrategy
    {
        private readonly decimal _flatAmount;

        public FlatDiscountStrategy(decimal flatAmount)
        {
            _flatAmount = flatAmount;
        }

        public decimal ApplyDiscount(decimal originalAmount) => Math.Max(0, originalAmount - _flatAmount);
    }
}
