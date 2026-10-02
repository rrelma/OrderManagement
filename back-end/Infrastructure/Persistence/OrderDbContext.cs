using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(builder =>
            {
                builder.HasKey(o => o.Id);

                // Mapping du Value Object: Money (TotalAmount)
                builder.OwnsOne(o => o.TotalAmount, money =>
                {
                    money.Property(m => m.Amount)
                         .HasColumnName("TotalAmount")
                         .HasColumnType("decimal(18,2)")
                         .IsRequired();

                    money.Property(m => m.Currency)
                         .HasColumnName("Currency")
                         .HasMaxLength(3)
                         .IsRequired();
                });

                // On ignore la propriété de domaine complexe State
                builder.Ignore(o => o.State);

                // Configuration de la Shadow Property _stateName (stockée sous forme de chaîne)
                builder.Property<string>("_stateName")
                       .HasColumnName("State")
                       .HasMaxLength(50)
                       .HasField("_stateName") 
                       .UsePropertyAccessMode(PropertyAccessMode.Field) 
                       .IsRequired();

                // Propriétés de l'entité Order
                builder.Property(o => o.CustomerId)
                       .IsRequired();

                builder.Property(o => o.CreatedAtUtc)
                       .IsRequired();
        });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Order>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    // Extrait le nom de l'état (ex: "PendingOrderState" -> "Pending")
                    var stateName = entry.Entity.State.GetType().Name.Replace("OrderState", "").Replace("State", "");

                    // Affecte la valeur à la Shadow Property '_stateName'
                    entry.Property<string>("_stateName").CurrentValue = stateName;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
