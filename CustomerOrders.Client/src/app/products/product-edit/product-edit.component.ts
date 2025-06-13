import {ChangeDetectionStrategy, Component, computed, inject, signal} from '@angular/core';
import {MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef} from '@angular/material/bottom-sheet';
import {NonNullableFormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {ProductService} from '../product.service';
import {ProductCreateDto, ProductDto} from '../product.data';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';

@Component({
  selector: 'app-product-edit',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInput, MatButton],
  templateUrl: './product-edit.component.html',
  styleUrl: './product-edit.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductEditComponent {
  productService = inject(ProductService);
  private _ref = inject(MatBottomSheetRef<ProductEditComponent>);
  data = signal(inject(MAT_BOTTOM_SHEET_DATA) as ProductDto | null);
  mode = computed(() => this.data() ? 'Edit': 'Create');

  private _fb = inject(NonNullableFormBuilder);
  form = this._fb.group({
    id: this.data()?.id,
    name: [this.data()?.name, Validators.required],
    price: [this.data()?.price, Validators.required]
  });

  create(): void {
    this.productService.create(this.form.value as ProductCreateDto).subscribe(customer => {
      this._ref.dismiss()
    })
  }
  update(): void {
    this.productService.update(this.form.value as ProductDto).subscribe(customer => {
      this._ref.dismiss()
    })
  }

  submit() {
    if (this.form.invalid) return;

    this.data() ? this.update() : this.create();
  }
}
