using Domain.Customers;

namespace Domain.Orders;

public class Order : BaseEntity
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    
    public List<OrderLine> OrderLines { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}