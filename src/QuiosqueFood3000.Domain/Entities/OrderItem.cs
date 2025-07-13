namespace QuiosqueFood3000.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public required Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal TotalValue { get; set; }
    public string? Observations { get; set; }
}