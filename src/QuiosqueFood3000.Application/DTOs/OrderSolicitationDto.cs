using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Api.DTOs;

public class OrderSolicitationDto
{
    public string? Id { get; set; }
    public TypeOfIdentification? TypeOfIdentification { get; set; }
    public Customer? Customer { get; set; }
    public Guid? AnonymousIdentification { get; set; }
    public OrderSolicitationStatus? OrderSolicitationStatus { get; set; }
    public List<OrderItem>? OrderItemsList { get; set; }
    public decimal? TotalValue { get; set; }
    public DateTime? InitialDate { get; set; }
    public DateTime? EndDate { get; set; }
}