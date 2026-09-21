using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public class PendingOrderState : IOrderState
    {
        public string StatusName => "Pending";

        public void Pay(Order order)
        {
            order.TransitionToState(new PaidOrderState());
        }

        public void Ship(Order order)
        {
            throw new InvalidOperationException("Cannot ship an order that has not been paid yet.");
        }

        public void Cancel(Order order)
        {
            order.TransitionToState(new CancelledOrderState());
        }
    }
}
