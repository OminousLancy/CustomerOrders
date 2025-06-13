import {ChangeDetectionStrategy, Component, computed, inject, signal} from '@angular/core';
import {MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef} from '@angular/material/bottom-sheet';
import {NonNullableFormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {CustomerService} from '../customer.service';
import {CustomerDto} from '../customer.data';

@Component({
  selector: 'app-customer-edit',
  imports: [
    ReactiveFormsModule, MatFormFieldModule, MatInput, MatButton
  ],
  templateUrl: './customer-edit.component.html',
  styleUrl: './customer-edit.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerEditComponent {
  customerService = inject(CustomerService);
  private _ref = inject(MatBottomSheetRef<CustomerEditComponent>);
  data = signal(inject(MAT_BOTTOM_SHEET_DATA) as CustomerDto | null);
  mode = computed(() => this.data() ? 'Edit': 'Create');

  private _fb = inject(NonNullableFormBuilder);
  form = this._fb.group({
    id: this.data()?.id,
    name: [this.data()?.name, Validators.required],
    lastName: [this.data()?.lastName, Validators.required],
    address: [this.data()?.address, Validators.required],
  });

  create(): void {
    this.customerService.create(this.form.value as CustomerDto).subscribe(customer => {
      this._ref.dismiss()
    })
  }
  update(): void {
    this.customerService.update(this.form.value as CustomerDto).subscribe(customer => {
      this._ref.dismiss()
    })
  }

  submit() {
    if (this.form.invalid) return;

    this.data() ? this.update() : this.create();
  }
}
