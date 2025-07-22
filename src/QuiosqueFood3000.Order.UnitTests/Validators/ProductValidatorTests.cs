
using FluentValidation.TestHelper;
using QuiosqueFood3000.Api.Validators;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using Xunit;

namespace QuiosqueFood3000.Order.UnitTests.Validators
{
    public class ProductValidatorTests
    {
        private readonly ProductValidator _validator;

        public ProductValidatorTests()
        {
            _validator = new ProductValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsNull()
        {
            var product = new Product { Name = null, Available = true, ProductCategory = ProductCategory.Sandwich, Value = 10 };
            var result = _validator.TestValidate(product);
            result.ShouldHaveValidationErrorFor(p => p.Name);
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsEmpty()
        {
            var product = new Product { Name = "", Available = true, ProductCategory = ProductCategory.Sandwich, Value = 10 };
            var result = _validator.TestValidate(product);
            result.ShouldHaveValidationErrorFor(p => p.Name).WithErrorMessage("O produto deve possuir um nome");
        }

        [Fact]
        public void ShouldHaveErrorWhenValueIsNegative()
        {
            var product = new Product { Name = "Test", Available = true, ProductCategory = ProductCategory.Sandwich, Value = -10 };
            var result = _validator.TestValidate(product);
            result.ShouldHaveValidationErrorFor(p => p.Value).WithErrorMessage("O produto deve ter o valor igual ou maior que 0");
        }

        [Fact]
        public void ShouldNotHaveErrorWhenProductIsValid()
        {
            var product = new Product { Name = "Test", Available = true, ProductCategory = ProductCategory.Sandwich, Value = 10 };
            var result = _validator.TestValidate(product);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
