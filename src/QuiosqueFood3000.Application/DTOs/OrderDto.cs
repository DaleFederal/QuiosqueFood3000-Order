using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Api.DTOs;

public class OrderDto
{
    public string? Id { get; set; }
    public TypeOfIdentification? TypeOfIdentification { get; set; }
    public Customer? Customer { get; set; }
    public Guid? AnonymousIdentification { get; set; }
    public OrderSolicitation? OrderSolicitation { get; set; }
    public OrderStatus? OrderStatus { get; set; }
    public List<OrderItem>? OrderItemsList { get; set; }
    public decimal? TotalValue { get; set; }
    public DateTime? InitialDate { get; set; }
    public DateTime? EndDate { get; set; }
    public PaymentStatus? PaymentStatus { get; set; }
}