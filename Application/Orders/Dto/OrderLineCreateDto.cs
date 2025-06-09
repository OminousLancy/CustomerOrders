using Application.Products.Dto;

namespace Application.Orders.Dto;

public class OrderLineCreateDto
{
    public int OrderId { get; set; }
    public ProductDto Product { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
}