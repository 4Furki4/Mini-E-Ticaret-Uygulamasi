import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './admin/layout/layout.component';
import { HomeComponent } from './ui/components/home/home.component';

const routes: Routes = [
  // ADMIN LAYER
  {
    path: 'admin', component: LayoutComponent, children: [
      { path: '', loadChildren: () => import('./admin/components/dashboard/dashboard.module').then(m => m.DashboardModule) },
      { path: 'products', loadChildren: () => import('./admin/components/products/products.module').then(m => m.ProductsModule) },
      { path: 'customers', loadChildren: () => import('./admin/components/customers/customers.module').then(m => m.CustomersModule) },
      { path: 'orders', loadChildren: () => import('./admin/components/orders/orders.module').then(m => m.OrdersModule) },
    ]
  },
  // UI LAYER
  { path: '', component: HomeComponent, },
  { path: 'basket', loadChildren: () => import('./ui/components/baskets/baskets.module').then(m => m.BasketsModule) },
  { path: 'products', loadChildren: () => import('./ui/components/products/products.module').then(m => m.ProductsModule) },
  { path: 'register', loadChildren: () => import('./ui/components/register/register.module').then(m => m.RegisterModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
