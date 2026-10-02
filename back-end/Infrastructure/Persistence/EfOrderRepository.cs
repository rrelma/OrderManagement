using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class EfOrderRepository : IOrderRepository, IOrderReadRepository
    {
        private readonly OrderDbContext _context;

        public EfOrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        // ------------------------------------------------------------------
        // Write Repository Operations (IOrderRepository)
        // ------------------------------------------------------------------

        public async Task SaveAsync(Order order, CancellationToken ct = default)
        {
            await _context.Orders.AddAsync(order, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Order order, CancellationToken ct = default)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(ct);
        }

        async Task<Order?> IOrderRepository.GetByIdAsync(Guid id, CancellationToken ct)
        {
            Order? x = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id, ct);
            return x;
        }

        // ------------------------------------------------------------------
        // Read Repository Operations (IOrderReadRepository)
        // ------------------------------------------------------------------

        async Task<OrderDetailsDto?> IOrderReadRepository.GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.Id == id)
                .Select(o => new OrderDetailsDto(
                    o.Id,
                    o.CustomerId,
                    o.TotalAmount.Amount,
                    o.TotalAmount.Currency,
                    EF.Property<string>(o, "_stateName"),
                    o.CreatedAtUtc
                ))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<OrderDetailsDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Select(o => new OrderDetailsDto(
                    o.Id,
                    o.CustomerId,
                    o.TotalAmount.Amount,
                    o.TotalAmount.Currency,
                    EF.Property<string>(o, "_stateName"),
                    o.CreatedAtUtc
                ))
                .ToListAsync(ct);
        }
    }
}
