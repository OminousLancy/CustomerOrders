import {ProductDto} from '../products/product.data';

export interface OrderDto {
  id: number;
  customerName: string;
  customerId: number;
  status: OrderStatus;
  statusDescription: string;
  created: string;
  updated: string;
  orderLines: OrderLineDto[];
}

export interface OrderCreateDto {
  customerId: number;
  orderLines: OrderLineCreateDto[];
}

export interface OrderLineDto {
  id: number;
  orderId: number;
  product: ProductDto;
  price: number;
  count: number;
}

export interface OrderLineCreateDto {
  orderId: number;
  product: ProductDto;
  price: number;
  count: number;
}

export enum OrderStatus {
  Created = 0,
  InProgress = 1,
  Completed = 2,
  Cancelled = 3,
}
