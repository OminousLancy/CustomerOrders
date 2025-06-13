export enum ReportType
{
  Week = 0,
  Month = 1
}
export interface CustomerReportDto {
  totalOrders: number;
  totalSum: number;
  mostOrderedProduct: string;
}
