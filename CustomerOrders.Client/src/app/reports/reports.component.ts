import {ChangeDetectionStrategy, Component, computed, inject, signal} from '@angular/core';
import {CustomerService} from '../customers/customer.service';
import {rxResource, toSignal} from '@angular/core/rxjs-interop';
import {ReportService} from './report.service';
import {MatButtonModule} from '@angular/material/button';
import {ReportType} from './report.data';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatSelectModule} from '@angular/material/select';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {CurrencyPipe} from '@angular/common';

@Component({
  selector: 'app-reports',
  imports: [
    MatButtonModule, MatFormFieldModule, MatSelectModule, FormsModule, ReactiveFormsModule, CurrencyPipe
  ],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ReportsComponent {
  customerService = inject(CustomerService);
  reportService = inject(ReportService);

  customers = toSignal(this.customerService.getList(1, 100));
  selectedCustomer = signal<number>(0);
  selectedReportType = signal<ReportType>(ReportType.Month);

  reportResource = rxResource({
    request: () => ({customerId: this.selectedCustomer(), reportType: this.selectedReportType()}),
    loader: ({request: {customerId, reportType}}) => this.reportService.getCustomerReport(customerId, reportType)
  });
  report = computed(() => this.reportResource.value())
  selectCustomer(id: number) {
    this.selectedCustomer.set(id);
  }

  selectReportType(reportType: ReportType) {
    this.selectedReportType.set(reportType);
  }

  protected readonly ReportType = ReportType;
}
