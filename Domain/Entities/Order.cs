using Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }
        public Money TotalAmount { get; private set; }
        public string Status { get; private set; }
        public DateTime CreatedAt { get; }

        public Order(Guid customerId, string currency)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer ID is required.", nameof(customerId));

            Id = Guid.NewGuid();
            CustomerId = customerId;
            TotalAmount = new Money(0, currency);
            Status = "Pending";
            CreatedAt = DateTime.UtcNow;
        }

        public void AddItem(decimal price, string currency)
        {
            var itemPrice = new Money(price, currency);
            TotalAmount = TotalAmount.Add(itemPrice);
        }

        public void MarkAsPaid()
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending orders can be marked as paid.");

            Status = "Paid";
        }

        public void ApplyDiscount(IDiscountStrategy discountStrategy)
        {
            var discountedAmount = discountStrategy.ApplyDiscount(TotalAmount.Amount);
            TotalAmount = new Money(discountedAmount, TotalAmount.Currency);
        }
    }
}
