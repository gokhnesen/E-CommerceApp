import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ShopRoutingModuleTsComponent } from './shop-routing.module.ts/shop-routing.module.ts.component';



@NgModule({
  declarations: [
    ShopComponent,
    ShopRoutingModuleTsComponent
  ],
  imports: [
    CommonModule
  ]
})
export class ShopModule { }
