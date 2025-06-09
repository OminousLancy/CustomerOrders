using Domain.Orders;

namespace Domain.Customers;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string? PhotoUrl { get; set; }

    public List<Order> Orders { get; set; } = [];
}