using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Api.DTOs;

public class PaymentDto
{
    public Guid PaymentId { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public int OrderId { get; set; }
}