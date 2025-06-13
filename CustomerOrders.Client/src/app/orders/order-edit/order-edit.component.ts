import {ChangeDetectionStrategy, Component, computed, DestroyRef, effect, inject, signal} from '@angular/core';
import {MatButton} from '@angular/material/button';
import {MatFormField, MatLabel} from '@angular/material/form-field';
import {FormArray, NonNullableFormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {CustomerService} from '../../customers/customer.service';
import {MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef} from '@angular/material/bottom-sheet';
import {OrderService} from '../order.service';
import {OrderDto, OrderLineDto, OrderStatus} from '../order.data';
import {MatOption, MatSelect} from '@angular/material/select';
import {takeUntilDestroyed, toSignal} from '@angular/core/rxjs-interop';
import {ProductDto} from '../../products/product.data';
import {ProductService} from '../../products/product.service';
import {MatInput} from '@angular/material/input';
import {MatIconModule} from '@angular/material/icon';
import {CurrencyPipe} from '@angular/common';
import {formArrayMinLengthValidator} from '../../core/validators/form-array.validators';

@Component({
  selector: 'app-order-edit',
  imports: [
    MatButton,
    MatFormField,
    MatLabel,
    ReactiveFormsModule,
    MatSelect,
    MatOption,
    MatInput,
    MatIconModule,
    CurrencyPipe,
  ],
  templateUrl: './order-edit.component.html',
  styleUrl: './order-edit.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrderEditComponent {
  orderService = inject(OrderService);
  customerService = inject(CustomerService);
  productService = inject(ProductService);
  private _fb = inject(NonNullableFormBuilder);
  private _ref = inject(MatBottomSheetRef<OrderEditComponent>);
  destroyRef = inject(DestroyRef);
  data = signal(inject(MAT_BOTTOM_SHEET_DATA) as OrderDto | null);
  mode = computed(() => this.data() ? 'Edit' : 'Create');

  canEdit = computed(() => this.data() === null ||
    this.data()?.status === OrderStatus.Created || this.data()?.status === OrderStatus.InProgress);

  customers = toSignal(this.customerService.getList(1, 100));
  products = toSignal(this.productService.getList(1, 100));

  form = this._fb.group({
    id: this.data()?.id || 0,
    status: this.data()?.status,
    customerId: [this.data()?.customerId, Validators.required],
    orderLines: this._fb.array<OrderLineDto[]>([], formArrayMinLengthValidator(1))
  });

  dataEffect = effect(() => {
    this.data()?.orderLines.forEach(item => this.orderLines.push(this.createOrderLineGroup(item)));
  })
  canEditEffect = effect(() => {
    if (!this.canEdit()) {
      this.form.disable();
    }
  })
  get orderLines(): FormArray {
    return this.form.controls.orderLines;
  }

  create(): void {
    this.orderService.create(this.form.value as OrderDto).subscribe(data => {
      this._ref.dismiss()
    })
  }

  update(): void {
    const model = {
      ...this.data(),
      ...this.form.value as OrderDto,
    }
    console.log("=>(order-edit.component.ts:79) model", model);
    this.orderService.update(model).subscribe(data => {
      this._ref.dismiss()
    })
  }

  submit() {
    if(!this.canEdit()) return;

    if (this.form.invalid) return;

    this.data() ? this.update() : this.create();
  }

  addOrderLine() {
    if(!this.canEdit()) return;
    let orderLine = this.createOrderLineGroup(null);
    this.orderLines.push(orderLine);
  }

  removeOrderLine(index: number) {
    if(!this.canEdit()) return;
    this.orderLines.removeAt(index);
  }

  compareObjects(c1: { id: number }, c2: { id: number }) {
    return c1 && c2 && c1.id === c2.id;
  }
  private createOrderLineGroup(orderLine: OrderLineDto | null) {
    let group;
    if (orderLine) {
      group = this._fb.group({
        id: orderLine.id,
        orderId: orderLine.orderId,
        product: orderLine.product,
        count: orderLine.count,
        price: orderLine.price,
      })
    } else {
      group = this._fb.group({
        id: 0,
        orderId: this.form.value.id,
        product: {} as ProductDto,
        count: 1,
        price: 0,
      })
    }
    group.valueChanges.pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(value => {
      if (value.product?.price && value.count) {
        group.controls.price.setValue(value.product.price * value.count, {emitEvent: false})
      }
    });
    return group;
  }

  protected readonly OrderStatus = OrderStatus;
}
