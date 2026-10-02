import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { OrderService } from '../../core/services/order.service';
import { CreateOrderCommand } from '../../core/models/order.model';


@Component({
  selector: 'app-order-entry',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './order-entry.component.html'
})
export class OrderEntryComponent {
  private readonly fb = inject(FormBuilder);
  private readonly orderService = inject(OrderService);

  // Signals for local UI state
  isSubmitting = signal(false);
  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  // Form initialization with sensible defaults
  orderForm = this.fb.group({
    customerId: ['3fa85f64-5717-4562-b3fc-2c963f66afa6', [Validators.required]],
    amount: [250.00, [Validators.required, Validators.min(0.01)]],
    currency: ['USD', [Validators.required]],
    discountType: ['Percentage', [Validators.required]],
    discountValue: [10, [Validators.min(0)]],
    paymentProvider: ['Stripe', [Validators.required]]
  });

  // Calculate estimated final price based on selected strategy
  get calculatedTotal(): number {
    const rawAmount = this.orderForm.value.amount || 0;
    const discountType = this.orderForm.value.discountType;
    const discountValue = this.orderForm.value.discountValue || 0;

    if (discountType === 'Percentage') {
      return Math.max(0, rawAmount - (rawAmount * (discountValue / 100)));
    } else if (discountType === 'flat') {
      return Math.max(0, rawAmount - discountValue);
    }
    return rawAmount;
  }

  onSubmit(): void {
    if (this.orderForm.invalid) {
      this.orderForm.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.successMessage.set(null);
    this.errorMessage.set(null);

    const command = this.orderForm.value as CreateOrderCommand;

    this.orderService.createOrder(command).subscribe({
      next: (response) => {
        this.isSubmitting.set(false);
        this.successMessage.set(`Order successfully created! ID: ${response.orderId}`);
      },
      error: (err) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(err.error?.error || 'Failed to submit order. Check backend connection.');
      }
    });
  }
}