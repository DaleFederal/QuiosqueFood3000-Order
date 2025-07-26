using FluentValidation;
using QuiosqueFood3000.Domain.Entities;

namespace QuiosqueFood3000.Api.Validators;
public class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
    {
        RuleFor(orderItem => orderItem.TotalValue).NotNull().Must(totalValue => totalValue >= 0)
            .WithMessage("O item de pedido deve possuir o valor igual ou maior que 0");

        RuleFor(orderItem => orderItem.Quantity).NotNull().Must(quantity => quantity > 0)
            .WithMessage("A quantidade de item de pedido deve ao menos 1");
    }
}