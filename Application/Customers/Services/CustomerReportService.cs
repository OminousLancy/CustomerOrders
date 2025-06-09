using Application.Customers.Dto;
using Application.Customers.Interfaces;
using Application.Report.Dto;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Services;

public class CustomerReportService(ApplicationDbContext context) : ICustomerReportService
{
    public async Task<CustomerReportDto> GetReportAsync(int customerId, ReportType reportType, CancellationToken ct = default)
    {
        DateTime from, to;
        if (reportType == ReportType.Week) {
            from = DateTime.UtcNow.AddDays(-7);
            to = DateTime.UtcNow;
        } else {
            from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            to = from.AddMonths(1);
        }

        var ordersQuery = context.Orders
            .Where(o => o.CustomerId == customerId && o.Created >= from && o.Created <= to);

        var totalOrders = await ordersQuery.CountAsync(o => o.CustomerId == customerId, ct);
        var totalSum = await ordersQuery
            .SelectMany(o => o.OrderLines)
            .SumAsync(ol => ol.Price, ct);
        
        var mostOrderedProduct = await ordersQuery
            .SelectMany(o => o.OrderLines)
            .GroupBy(ol => ol.Product.Name)
            .Select(g => new { ProductName = g.Key, TotalCount = g.Sum(x => x.Count) })
            .OrderByDescending(g => g.TotalCount)
            .Select(g => g.ProductName)
            .FirstOrDefaultAsync(ct) ?? "N/A";

        return new CustomerReportDto {
            TotalOrders = totalOrders,
            TotalSum = totalSum,
            MostOrderedProduct = mostOrderedProduct
        };
    }
}