using FluentValidation;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Helpers;

namespace QuiosqueFood3000.Api.Validators;
public class CustomerDtoValidator : AbstractValidator<CustomerDto>
{
    public CustomerDtoValidator()
    {
        RuleFor(customerDto => customerDto.Name).NotNull();
        RuleFor(customerDto => customerDto.Cpf).NotNull().NotEmpty().Must(cpf => CpfHelper.IsValidCpf(cpf)).WithMessage("O CPF não está válido");
        RuleFor(customerDto => customerDto.Email).NotNull().NotEmpty().Must(email => EmailHelper.IsValidEmail(email)).WithMessage("O E-mail não está válido");

    }
}
