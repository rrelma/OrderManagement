using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public MoneyDto TotalAmount { get; set; } = default!;
        public string State { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
    }
}
