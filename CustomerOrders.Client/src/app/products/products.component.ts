import {ChangeDetectionStrategy, Component, inject, OnInit, signal} from '@angular/core';
import {MatButtonModule} from "@angular/material/button";
import {
  MatTableModule
} from "@angular/material/table";
import {MatPaginatorModule, PageEvent} from "@angular/material/paginator";
import {MatBottomSheet} from '@angular/material/bottom-sheet';
import {CustomerDto} from '../customers/customer.data';
import {map, tap} from 'rxjs';
import {CustomerEditComponent} from '../customers/customer-edit/customer-edit.component';
import {ProductService} from './product.service';
import {MatIconModule} from '@angular/material/icon';
import {ProductDto} from './product.data';
import {ProductEditComponent} from './product-edit/product-edit.component';
import {CurrencyPipe} from '@angular/common';

@Component({
  selector: 'app-products',
  imports: [
    MatTableModule, MatButtonModule, MatPaginatorModule, MatIconModule, CurrencyPipe
  ],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductsComponent implements OnInit {
  private _bottomSheet = inject(MatBottomSheet);
  displayedColumns: string[] = ['id', 'name', 'price', 'actions'];
  productService = inject(ProductService);
  page = signal(1);
  take = signal(10);
  totalCount = signal<number>(0);
  products = signal<ProductDto[]>([]);

  ngOnInit() {
    this.search()
  }

  pageChanged(event: PageEvent) {
    this.page.set(event.pageIndex+1);
    this.take.set(event.pageSize);
  }
  search() {
    this.productService.getList(this.page(), this.take()).pipe(
      tap((res) => {
        this.totalCount.set(res.totalCount);
      }),
      map(res => res.items ),
    ).subscribe(customers => {
      this.products.set(customers);
    });
  }

  create() {
    this._bottomSheet.open(ProductEditComponent, {data: null}).afterDismissed().subscribe(res => {
      this.search()
    })
  }

  edit(element: CustomerDto) {
    this._bottomSheet.open(ProductEditComponent, {data: element}).afterDismissed().subscribe(res => {
      this.search()
    })
  }
}
