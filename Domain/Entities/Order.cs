using Domain.Events;
using Domain.States;
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

        private readonly List<IDomainEvent> _domainEvents = new();

        public Guid Id { get; }
        public Guid CustomerId { get; }
        public Money TotalAmount { get; private set; }
        public IOrderState State { get; private set; }
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public Order(Guid customerId, string currency)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer ID is required.", nameof(customerId));

            Id = Guid.NewGuid();
            CustomerId = customerId;
            TotalAmount = new Money(0, currency);
            State = new PendingOrderState(); // Initial State

            // Raise Domain Event
            _domainEvents.Add(new OrderPlacedDomainEvent(Id, CustomerId, TotalAmount.Amount));
        }

        public void AddItem(decimal price, string currency)
        {
            var itemPrice = new Money(price, currency);
            TotalAmount = TotalAmount.Add(itemPrice);
        }

        public void TransitionToState(IOrderState newState)
        {
            State = newState;
        }

        public void ApplyDiscount(IDiscountStrategy discountStrategy)
        {
            var discountedAmount = discountStrategy.ApplyDiscount(TotalAmount.Amount);
            TotalAmount = new Money(discountedAmount, TotalAmount.Currency);
        }

        public void MarkAsPaid() => State.Pay(this);
        public void Ship() => State.Ship(this);
        public void Cancel() => State.Cancel(this);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
