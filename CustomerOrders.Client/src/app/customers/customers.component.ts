import {ChangeDetectionStrategy, Component, inject, OnInit, signal} from '@angular/core';
import {MatTableModule} from '@angular/material/table';
import {map, tap} from 'rxjs';
import {MatPaginator, PageEvent} from '@angular/material/paginator';
import {MatButton, MatIconButton} from '@angular/material/button';
import {MatBottomSheet} from '@angular/material/bottom-sheet';
import {CustomerEditComponent} from './customer-edit/customer-edit.component';
import {MatIcon} from '@angular/material/icon';
import {CustomerService} from './customer.service';
import {CustomerDto} from './customer.data';

@Component({
  selector: 'app-customers',
  imports: [
    MatTableModule,
    MatPaginator,
    MatButton,
    MatIcon,
    MatIconButton,
  ],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomersComponent implements OnInit {
  private _bottomSheet = inject(MatBottomSheet);
  displayedColumns: string[] = ['id', 'name', 'lastName', 'address', 'actions'];
  customerService = inject(CustomerService);
  page = signal(1);
  take = signal(10);
  totalCount = signal<number>(0);
  customers = signal<CustomerDto[]>([]);

  ngOnInit() {
    this.search()
  }

  pageChanged(event: PageEvent) {
    this.page.set(event.pageIndex+1);
    this.take.set(event.pageSize);
  }
  search() {
    this.customerService.getList(this.page(), this.take()).pipe(
      tap((res) => {
        this.totalCount.set(res.totalCount);
      }),
      map(res => res.items ),
    ).subscribe(customers => {
      this.customers.set(customers);
    });
  }

  create() {
    this._bottomSheet.open(CustomerEditComponent, {data: null}).afterDismissed().subscribe(res => {
      this.search()
    })
  }

  edit(element: CustomerDto) {
    this._bottomSheet.open(CustomerEditComponent, {data: element}).afterDismissed().subscribe(res => {
      this.search()
    })
  }
}
