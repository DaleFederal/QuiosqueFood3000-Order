using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool Available { get; set; }
    public ProductCategory ProductCategory { get; set; }
    public decimal Value { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }

}