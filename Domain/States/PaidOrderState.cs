using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public class PaidOrderState : IOrderState
    {
        public string StatusName => "Paid";

        public void Pay(Order order)
        {
            throw new InvalidOperationException("Order is already paid.");
        }

        public void Ship(Order order)
        {
            order.TransitionToState(new ShippedOrderState());
        }

        public void Cancel(Order order)
        {
            throw new InvalidOperationException("Cannot cancel an order that is already paid.");
        }
    }
}
