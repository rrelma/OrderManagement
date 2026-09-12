using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderRepository
    {
        Task SaveAsync(Order order, CancellationToken ct = default);
    }
}
