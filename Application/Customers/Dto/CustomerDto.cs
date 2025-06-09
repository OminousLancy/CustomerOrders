namespace Application.Customers.Dto;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string? PhotoUrl { get; set; }
}