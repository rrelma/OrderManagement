import { Routes } from '@angular/router';
import { OrderEntryComponent } from './features/order-entry/order-entry.component';
import { OrderListComponent } from './features/order-list/order-list.component';

export const routes: Routes = [
  { path: '', redirectTo: 'create', pathMatch: 'full' },
  { path: 'create', component: OrderEntryComponent },
  { path: 'orders', component: OrderListComponent }
];