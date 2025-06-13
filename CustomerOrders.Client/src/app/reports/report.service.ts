import {inject, Injectable} from '@angular/core';
import {API_BASE_URL} from '../core/tokens/api-base-url.token';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {CustomerReportDto, ReportType} from './report.data';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private baseUrl = inject(API_BASE_URL) + '/api/Report';
  private http = inject(HttpClient);

  constructor() {}

  getCustomerReport(customerId: number, reportType: ReportType): Observable<CustomerReportDto> {
    return this.http.get<CustomerReportDto>(`${this.baseUrl}/GetCustomerReport?customerId=${customerId}&reportType=${reportType}`);
  }
}
