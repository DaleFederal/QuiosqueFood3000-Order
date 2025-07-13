using QuiosqueFood3000.Domain.Entities;

namespace QuiosqueFood3000.Api.DTOs
{
    public class OrderItemDto
    {
        public string? Id { get; set; }
        public required Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }
        public string? Observations { get; set; }
    }
}
