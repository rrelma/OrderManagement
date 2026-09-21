using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public class CancelledOrderState : IOrderState
    {
        public string StatusName => "Cancelled";

        public void Pay(Order order) => throw new InvalidOperationException("Cannot pay a cancelled order.");
        public void Ship(Order order) => throw new InvalidOperationException("Cannot ship a cancelled order.");
        public void Cancel(Order order) => throw new InvalidOperationException("Order is already cancelled.");
    }
}
