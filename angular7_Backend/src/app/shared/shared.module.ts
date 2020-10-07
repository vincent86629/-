import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilterPipe } from '../pipe/filter.pipe';
import { OrderByPipe } from '../pipe/order-by.pipe';


@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    FilterPipe,
    OrderByPipe
  ],
  exports: [
    FilterPipe,
    OrderByPipe
]
})
export class SharedModule { }
