import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService } from '../../core/services/order.service';
import { Order, OrderDto } from '../../core/models/order.model';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-list.component.html'
})
export class OrderListComponent implements OnInit {
  private readonly orderService = inject(OrderService);

  // Signals for managing UI state
  orders = signal<OrderDto[]>([]);
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);
  actionMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.orderService.getOrders().subscribe({
      next: (data) => {
        this.orders.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Failed to load orders. Make sure the backend API is running.');
        this.isLoading.set(false);
      }
    });
  }

  shipOrder(orderId: string): void {
    this.actionMessage.set(null);

    this.orderService.shipOrder(orderId).subscribe({
      next: () => {
        this.actionMessage.set(`Order #${orderId.substring(0, 8)}... marked as Shipped!`);
        this.loadOrders(); // Refresh list to get updated state
      },
      error: (err) => {
        console.log('Error shipping order:', err);
        this.errorMessage.set(err.error?.error || 'Cannot ship order. Check if order state is Paid.');
      }
    });
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Pending': return 'bg-yellow-100 text-yellow-800';
      case 'Paid': return 'bg-blue-100 text-blue-800';
      case 'Shipped': return 'bg-green-100 text-green-800';
      case 'Cancelled': return 'bg-red-100 text-red-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  }
}