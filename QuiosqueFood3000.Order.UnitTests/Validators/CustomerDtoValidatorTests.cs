
using FluentValidation.TestHelper;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Validators;
using Xunit;

namespace QuiosqueFood3000.Order.UnitTests.Validators
{
    public class CustomerDtoValidatorTests
    {
        private readonly CustomerDtoValidator _validator;

        public CustomerDtoValidatorTests()
        {
            _validator = new CustomerDtoValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsNull()
        {
            var customerDto = new CustomerDto { Name = null, Cpf = "11144477735", Email = "test@test.com" };
            var result = _validator.TestValidate(customerDto);
            result.ShouldHaveValidationErrorFor(customerDto => customerDto.Name);
        }

        [Fact]
        public void ShouldHaveErrorWhenCpfIsNull()
        {
            var customerDto = new CustomerDto { Name = "Test", Cpf = null, Email = "test@test.com" };
            var result = _validator.TestValidate(customerDto);
            result.ShouldHaveValidationErrorFor(customerDto => customerDto.Cpf);
        }

        [Fact]
        public void ShouldHaveErrorWhenCpfIsEmpty()
        {
            var customerDto = new CustomerDto { Name = "Test", Cpf = "", Email = "test@test.com" };
            var result = _validator.TestValidate(customerDto);
            result.ShouldHaveValidationErrorFor(customerDto => customerDto.Cpf);
        }

        [Fact]
        public void ShouldHaveErrorWhenCpfIsInvalid()
        {
            var customerDto = new CustomerDto { Name = "Test", Cpf = "123", Email = "test@test.com" };
            var result = _validator.TestValidate(customerDto);
            result.ShouldHaveValidationErrorFor(customerDto => customerDto.Cpf).WithErrorMessage("O CPF não está válido");
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsNull()
        {
            var customerDto = new CustomerDto { Name = "Test", Cpf = "11144477735", Email = null };
            var result = _validator.TestValidate(customerDto);
            result.ShouldHaveValidationErrorFor(customerDto => customerDto.Email);
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsEmpty()
        {
            var customerDto = new CustomerDto { Name = "Test", Cpf = "11144477735", Email = "" };
            var result = _validator.TestValidate(customerDto);
            result.ShouldHaveValidationErrorFor(customerDto => customerDto.Email);
        }

        [Fact]
        public void ShouldHaveErrorWhenEmailIsInvalid()
        {
            var customerDto = new CustomerDto { Name = "Test", Cpf = "11144477735", Email = "invalid-email" };
            var result = _validator.TestValidate(customerDto);
            result.ShouldHaveValidationErrorFor(customerDto => customerDto.Email).WithErrorMessage("O E-mail não está válido");
        }

        [Fact]
        public void ShouldNotHaveErrorWhenCustomerDtoIsValid()
        {
            var customerDto = new CustomerDto { Name = "Test", Cpf = "11144477735", Email = "test@test.com" };
            var result = _validator.TestValidate(customerDto);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
