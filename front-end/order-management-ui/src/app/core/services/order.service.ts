import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Order, CreateOrderCommand, OrderDto } from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/orders`;

  // GET /api/orders
  getOrders(): Observable<OrderDto[]> {
    return this.http.get<OrderDto[]>(this.apiUrl);
  }

  // GET /api/orders/{id}
  getOrderById(id: string): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${id}`);
  }

  // POST /api/orders
  createOrder(command: CreateOrderCommand): Observable<{ orderId: string }> {
    return this.http.post<{ orderId: string }>(this.apiUrl, command);
  }

  // PUT /api/orders/{id}/ship
  shipOrder(id: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/ship`, {});
  }
}