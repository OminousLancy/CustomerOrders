import {ChangeDetectionStrategy, Component, inject, signal} from '@angular/core';
import {MatBottomSheet} from '@angular/material/bottom-sheet';
import {MatPaginator, PageEvent} from '@angular/material/paginator';
import {map, tap} from 'rxjs';
import {OrderService} from './order.service';
import {OrderDto} from './order.data';
import {MatButton, MatIconButton} from '@angular/material/button';
import {MatTableModule
} from '@angular/material/table';
import {DatePipe, NgClass} from '@angular/common';
import {MatIcon} from '@angular/material/icon';
import {OrderLineComponent} from './order-line/order-line.component';
import {OrderEditComponent} from './order-edit/order-edit.component';

@Component({
  selector: 'app-orders',
  imports: [
    MatTableModule,
    MatPaginator,
    MatButton,
    MatIcon,
    MatIconButton,
    DatePipe,
    OrderLineComponent,
    NgClass,
  ],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrdersComponent {
  private _bottomSheet = inject(MatBottomSheet);
  displayedColumns: string[] = ['id', 'customerName', 'status', 'created', 'updated', 'actions'];
  columnsToDisplayWithExpand = [...this.displayedColumns, 'expand'];
  expandedElement: OrderDto | null = null;
  orderService = inject(OrderService);
  page = signal(1);
  take = signal(10);
  totalCount = signal<number>(0);
  orders = signal<OrderDto[]>([]);
  /** Checks whether an element is expanded. */
  isExpanded(element: OrderDto) {
    return this.expandedElement === element;
  }

  /** Toggles the expanded state of an element. */
  toggle(element: OrderDto) {
    this.expandedElement = this.isExpanded(element) ? null : element;
  }
  ngOnInit() {
    this.search()
  }

  pageChanged(event: PageEvent) {
    this.page.set(event.pageIndex+1);
    this.take.set(event.pageSize);
  }
  search() {
    this.orderService.getList(this.page(), this.take()).pipe(
      tap((res) => {
        this.totalCount.set(res.totalCount);
      }),
      map(res => res.items ),
    ).subscribe(customers => {
      this.orders.set(customers);
    });
  }

  create() {
    this._bottomSheet.open(OrderEditComponent, {data: null}).afterDismissed().subscribe(res => {
      this.search()
    })
  }

  edit(element: OrderDto) {
    this._bottomSheet.open(OrderEditComponent, {data: element}).afterDismissed().subscribe(res => {
      this.search()
    })
  }
}
