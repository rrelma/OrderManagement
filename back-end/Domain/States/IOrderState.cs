using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.States
{
    public interface IOrderState
    {
        string StatusName { get; }
        void Pay(Order order);
        void Ship(Order order);
        void Cancel(Order order);
    }
}
