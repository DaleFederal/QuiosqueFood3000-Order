namespace QuiosqueFood3000.Api.DTOs;

public class KitchenOrderSolicitationDto
{
    public Guid Id { get; set; }
    public string Status { get; set; }
    public DateTime GenerateDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public Guid? CustomerId { get; set; }
    public string? AnonymousIdentification { get; set; }
    public List<KitchenProductDto> Products { get; set; } = new();
}
