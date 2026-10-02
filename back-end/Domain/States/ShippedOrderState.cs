using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public class ShippedOrderState : IOrderState
    {
        public string StatusName => "Shipped";

        public void Pay(Order order) => throw new InvalidOperationException("Cannot pay a shipped order.");
        public void Ship(Order order) => throw new InvalidOperationException("Order is already shipped.");
        public void Cancel(Order order) => throw new InvalidOperationException("Cannot cancel a shipped order.");
    }
}
