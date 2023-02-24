import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeModule } from './home/home.module';
import { ProductsModule } from './products/products.module';
import { BasketsModule } from './baskets/baskets.module';
import { RegisterModule } from './register/register.module';



@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    HomeModule,
    ProductsModule,
    BasketsModule,
    RegisterModule
  ]
})
export class ComponentsModule { }
