using Application.Customers.Dto;
using Application.Report.Dto;

namespace Application.Customers.Interfaces;

public interface ICustomerReportService
{
    Task<CustomerReportDto> GetReportAsync(int customerId, ReportType reportType, CancellationToken ct = default);
}