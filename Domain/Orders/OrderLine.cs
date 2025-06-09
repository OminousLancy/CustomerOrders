using Domain.Products;

namespace Domain.Orders;

public class OrderLine : BaseEntity 
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public int Count { get; set; }
    public decimal Price { get; set; }
}