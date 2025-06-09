namespace Application.Orders.Dto;

public class OrderCreateDto
{
    public int CustomerId { get; set; }
    public List<OrderLineDto> OrderLines { get; set; }
}