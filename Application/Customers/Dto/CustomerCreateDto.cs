using System.ComponentModel.DataAnnotations;

namespace Application.Customers.Dto;

public class CustomerCreateDto
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string? PhotoUrl { get; set; }
}