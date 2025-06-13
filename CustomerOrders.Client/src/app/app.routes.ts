import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'customers',
  },
  {
    path: 'customers',
    loadComponent: () => import('./customers/customers.component').then(c => c.CustomersComponent)
  },
  {
    path: 'products',
    loadComponent: () => import('./products/products.component').then(c => c.ProductsComponent)
  },
  {
    path: 'orders',
    loadComponent: () => import('./orders/orders.component').then(c => c.OrdersComponent)
  },
  {
    path: 'reports',
    loadComponent: () => import('./reports/reports.component').then(c => c.ReportsComponent)
  }
];
