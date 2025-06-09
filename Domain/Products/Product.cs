namespace Domain.Products;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}