import {inject, Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {CustomerDto} from './customer.data';
import {PaginatedResponse} from '../core/models/api.data';
import {HttpClient} from '@angular/common/http';
import {API_BASE_URL} from '../core/tokens/api-base-url.token';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private baseUrl = inject(API_BASE_URL) + '/api/Customer';
  private http = inject(HttpClient);
  constructor() { }
  getList(page: number, take: number): Observable<PaginatedResponse<CustomerDto>> {
    return this.http.get<PaginatedResponse<CustomerDto>>(`${this.baseUrl}/GetList?page=${page}&take=${take}`);
  }

  getById(id: number): Observable<CustomerDto> {
    return this.http.get<CustomerDto>(`${this.baseUrl}/getById?id=${id}`);
  }

  create(customer: CustomerDto): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}/Create`, customer);
  }

  update(customer: CustomerDto): Observable<CustomerDto> {
    return this.http.post<CustomerDto>(`${this.baseUrl}/Update`, customer);
  }
}
