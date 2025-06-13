import {inject, Injectable } from '@angular/core';
import {API_BASE_URL} from '../core/tokens/api-base-url.token';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import { PaginatedResponse } from '../core/models/api.data';
import {OrderCreateDto, OrderDto, OrderLineCreateDto, OrderStatus} from './order.data';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private baseUrl = inject(API_BASE_URL) + '/api/Order';
  private http = inject(HttpClient);

  constructor() {}

  getList(page: number, take: number): Observable<PaginatedResponse<OrderDto>> {
    return this.http.get<PaginatedResponse<OrderDto>>(`${this.baseUrl}/GetList?page=${page}&take=${take}`);
  }

  create(order: OrderCreateDto): Observable<OrderDto> {
    return this.http.post<OrderDto>(`${this.baseUrl}/Create`, order);
  }
  update(order: OrderCreateDto): Observable<OrderDto> {
    return this.http.post<OrderDto>(`${this.baseUrl}/Update`, order);
  }

  changeStatus(id: number, status: OrderStatus): Observable<OrderStatus> {
    return this.http.post<OrderStatus>(`${this.baseUrl}/ChangeStatus?id=${id}&status=${status}`, {});
  }

  addOrderLine(dto: OrderLineCreateDto): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}/AddOrderLine`, dto);
  }

  deleteOrderLine(orderId: number, orderLineId: number): Observable<string> {
    return this.http.delete<string>(`${this.baseUrl}/DeleteOrderLine?orderId=${orderId}&orderLineId=${orderLineId}`);
  }
}
