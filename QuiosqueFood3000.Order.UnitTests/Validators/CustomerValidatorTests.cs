
using FluentValidation.TestHelper;
using QuiosqueFood3000.Api.Helpers;
using QuiosqueFood3000.Api.Validators;
using QuiosqueFood3000.Domain.Entities;
using Xunit;

namespace QuiosqueFood3000.Order.UnitTests.Validators
{
    public class CustomerValidatorTests
    {
        private readonly CustomerValidator _validator;

        public CustomerValidatorTests()
        {
            _validator = new CustomerValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsNull()
        {
            var customer = new Customer { Name = null, Cpf = "11144477735", Email = "test@test.com" };
            var result = _validator.TestValidate(customer);
            result.ShouldHaveValidationErrorFor(customer => customer.Name);
        }

        [Fact]
        public void ShouldHaveErrorWhenCpfIsNull()
        {
            var customer = new Customer { Name = "Test", Cpf = null, Email = "test@test.com" };
            var result = _validator.TestValidate(customer);
            result.ShouldHaveValidationErrorFor(customer => customer.Cpf);
        }

        [Fact]
        public void ShouldHaveErrorWhenCpfIsEmpty()
        {
            var customer = new Customer { Name = "Test", Cpf = "", Email = "test@test.com" };
            var result = _validator.TestValidate(customer);
            result.ShouldHaveValidationErrorFor(customer => customer.Cpf);
        }

        [Fact]
        public void ShouldHaveErrorWhenCpfIsInvalid()
        {
            var customer = new Customer { Name = "Test", Cpf = "123", Email = "test@test.com" };
            var result = _validator.TestValidate(customer);
            result.ShouldHaveValidationErrorFor(customer => customer.Cpf).WithErrorMessage("O CPF não está inválido");
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsNull()
        {
            var customer = new Customer { Name = "Test", Cpf = "11144477735", Email = null };
            var result = _validator.TestValidate(customer);
            result.ShouldHaveValidationErrorFor(customer => customer.Email);
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsEmpty()
        {
            var customer = new Customer { Name = "Test", Cpf = "11144477735", Email = "" };
            var result = _validator.TestValidate(customer);
            result.ShouldHaveValidationErrorFor(customer => customer.Email);
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsInvalid()
        {
            var customer = new Customer { Name = "Test", Cpf = "11144477735", Email = "invalid-email" };
            var result = _validator.TestValidate(customer);
            result.ShouldHaveValidationErrorFor(customer => customer.Email).WithErrorMessage("O E-mail não está válido");
        }

        [Fact]
        public void ShouldNotHaveErrorWhenCustomerIsValid()
        {
            var customer = new Customer { Name = "Test", Cpf = "11144477735", Email = "test@test.com" };
            var result = _validator.TestValidate(customer);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
