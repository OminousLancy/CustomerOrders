import {ChangeDetectionStrategy, Component, input} from '@angular/core';
import {OrderLineDto} from '../order.data';
import {CurrencyPipe} from '@angular/common';
import {MatCardModule} from '@angular/material/card';
import {MatButtonModule} from '@angular/material/button';
import { MatIconModule} from '@angular/material/icon';

@Component({
  selector: 'app-order-line',
  imports: [
    MatCardModule,
    CurrencyPipe,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './order-line.component.html',
  styleUrl: './order-line.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrderLineComponent {
  orderLine = input.required<OrderLineDto>();
}
