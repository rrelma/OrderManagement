using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record OrderDetailsDto(Guid Id, Guid CustomerId, decimal TotalAmount, string Currency, string Status, DateTime CreatedAt);
}
