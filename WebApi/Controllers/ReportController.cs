using Application.Customers.Dto;
using Application.Customers.Interfaces;
using Application.Report.Dto;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ReportController(ICustomerReportService customerReportService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<CustomerReportDto>> GetCustomerReport(int customerId, ReportType reportType, CancellationToken ct = default) 
    {
        return Ok(await customerReportService.GetReportAsync(customerId, reportType, ct));
    }
}