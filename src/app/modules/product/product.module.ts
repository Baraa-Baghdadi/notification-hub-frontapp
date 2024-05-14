import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductTableComponent } from './components/product-table/product-table.component';
import {MatIconModule} from '@angular/material/icon';
import {MatTableModule} from '@angular/material/table';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    ProductTableComponent
  ],
  imports: [
    CommonModule,
    MatIconModule,
    MatTableModule,
    HttpClientModule
  ],
  exports : [ProductTableComponent]
})
export class ProductModule { }
