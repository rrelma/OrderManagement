using Domain.Entities;
using Domain.States;
using Domain.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Builders
{
    public class OrderBuilder
    {
        private Guid _customerId = Guid.NewGuid();
        private string _currency = "USD";
        private readonly List<(decimal Amount, string Currency)> _items = new();
        private IDiscountStrategy? _discountStrategy;
        private IOrderState? _initialState;

        public OrderBuilder WithCustomerId(Guid customerId)
        {
            _customerId = customerId;
            return this;
        }

        public OrderBuilder WithCurrency(string currency)
        {
            _currency = currency;
            return this;
        }

        public OrderBuilder AddItem(decimal amount, string? currency = null)
        {
            _items.Add((amount, currency ?? _currency));
            return this;
        }

        public OrderBuilder WithDiscount(IDiscountStrategy discountStrategy)
        {
            _discountStrategy = discountStrategy;
            return this;
        }

        public OrderBuilder WithState(IOrderState state)
        {
            _initialState = state;
            return this;
        }

        public Order Build()
        {
            var order = new Order(_customerId, _currency);

            foreach (var item in _items)
            {
                order.AddItem(item.Amount, item.Currency);
            }

            if (_discountStrategy != null)
            {
                order.ApplyDiscount(_discountStrategy);
            }

            if (_initialState != null)
            {
                order.TransitionToState(_initialState);
            }

            return order;
        }
    }
}
