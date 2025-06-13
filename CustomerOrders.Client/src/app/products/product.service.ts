import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../core/tokens/api-base-url.token';
import { ProductDto, ProductCreateDto } from './product.data';
import { PaginatedResponse } from '../core/models/api.data';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private baseUrl = inject(API_BASE_URL) + '/api/Product';
  private http = inject(HttpClient);

  constructor() {}

  getList(page: number, take: number): Observable<PaginatedResponse<ProductDto>> {
    return this.http.get<PaginatedResponse<ProductDto>>(`${this.baseUrl}/GetList?page=${page}&take=${take}`);
  }

  create(product: ProductCreateDto): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}/Create`, product);
  }

  update(product: ProductDto): Observable<ProductDto> {
    return this.http.post<ProductDto>(`${this.baseUrl}/Update`, product);
  }
}
