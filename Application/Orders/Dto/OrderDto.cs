using Application.Extensions;
using Domain.Orders;

namespace Application.Orders.Dto;

public class OrderDto
{   
    public int Id { get; set; }
    
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    
    public List<OrderLineDto> OrderLines { get; set; }
    public OrderStatus Status { get; set; }
    public string StatusDescription => Status.GetDescription();
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}