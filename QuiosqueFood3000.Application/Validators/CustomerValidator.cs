using FluentValidation;
using QuiosqueFood3000.Api.Helpers;
using QuiosqueFood3000.Domain.Entities;

namespace QuiosqueFood3000.Api.Validators;
public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(customer => customer.Name).NotNull();
        RuleFor(customer => customer.Cpf).NotNull().NotEmpty().Must(cpf => CpfHelper.IsValidCpf(cpf)).WithMessage("O CPF não está inválido");
        RuleFor(customer => customer.Email).NotNull().NotEmpty().Must(email => EmailHelper.IsValidEmail(email)).WithMessage("O E-mail não está válido");
    }
}