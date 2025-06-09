using Domain.Orders;

namespace Application.Orders.Dto;

public class OrderDto
{   
    public int Id { get; set; }
    
    public string CustomerName { get; set; }
    
    public List<OrderLineDto> OrderLines { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}