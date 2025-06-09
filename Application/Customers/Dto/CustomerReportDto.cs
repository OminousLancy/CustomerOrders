using Application.Products.Dto;
using Domain.Orders;

namespace Application.Customers.Dto;

public class CustomerReportDto
{
    public int TotalOrders { get; set; }
    public decimal TotalSum { get; set; }
    public string MostOrderedProduct { get; set; }
}