using FluentValidation;
using QuiosqueFood3000.Api.DTOs;

namespace QuiosqueFood3000.Api.Validators;
public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(product => product.Name).NotNull().NotEmpty().WithMessage("O produto deve possuir um nome");
        RuleFor(product => product.Available).NotNull().NotEmpty().Must(available => available == true || available == false).WithMessage("O campo Available deve ser true ou false");
        RuleFor(product => product.ProductCategory).NotNull().IsInEnum().WithMessage("A categoria não é válida para o produto");
        RuleFor(product => product.Value).NotNull().NotEmpty().Must(value => value >= 0).WithMessage("O produto deve ter o valor igual ou maior que 0");
    }
}