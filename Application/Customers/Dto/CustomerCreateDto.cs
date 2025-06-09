using System.ComponentModel.DataAnnotations;

namespace Application.Customers.Dto;

public class CustomerCreateDto
{
    [Required] public string Name { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string Address { get; set; }
    public string? PhotoUrl { get; set; }
}