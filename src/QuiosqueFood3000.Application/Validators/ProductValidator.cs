using FluentValidation;
using QuiosqueFood3000.Domain.Entities;

namespace QuiosqueFood3000.Api.Validators;
public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(product => product.Name).NotNull().NotEmpty().WithMessage("O produto deve possuir um nome");
        RuleFor(product => product.ProductCategory).NotNull().IsInEnum().WithMessage("A categoria não é válida para o produto");
        RuleFor(product => product.Value).NotNull().NotEmpty().Must(value => value >= 0).WithMessage("O produto deve ter o valor igual ou maior que 0");
    }
}