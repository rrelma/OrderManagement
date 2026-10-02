export type OrderStatus = 'Pending' | 'Paid' | 'Shipped' | 'Cancelled';
export type DiscountType = 'None' | 'Percentage' | 'FixedAmount';
export type PaymentProvider = 'Stripe' | 'PayPal';

export interface Money {
  amount: number;
  currency: string;
}

export interface OrderItem {
  id?: string;
  price: Money;
}

export interface OrderDto {
  id: string;
  customerId: string;
  totalAmount: number; // Flat primitive returned by backend
  currency?: string;
  status: OrderStatus | string;
  createdAtUtc?: string;
}

// Rich Domain Order (used internally when needed)
export interface Order {
  id: string;
  customerId: string;
  totalAmount: Money;
  status: OrderStatus;
  items: OrderItem[];
}

export interface CreateOrderCommand {
  customerId: string;
  amount: number;
  currency: string;
  discountType: DiscountType;
  discountValue: number;
  paymentProvider: PaymentProvider;
}