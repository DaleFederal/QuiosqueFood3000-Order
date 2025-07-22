
using FluentValidation.TestHelper;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Validators;
using QuiosqueFood3000.Domain.Entities.Enums;
using Xunit;

namespace QuiosqueFood3000.Order.UnitTests.Validators
{
    public class ProductDtoValidatorTests
    {
        private readonly ProductDtoValidator _validator;

        public ProductDtoValidatorTests()
        {
            _validator = new ProductDtoValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsNull()
        {
            var productDto = new ProductDto { Name = null, Available = true, ProductCategory = ProductCategory.Sandwich, Value = 10 };
            var result = _validator.TestValidate(productDto);
            result.ShouldHaveValidationErrorFor(p => p.Name);
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsEmpty()
        {
            var productDto = new ProductDto { Name = "", Available = true, ProductCategory = ProductCategory.Sandwich, Value = 10 };
            var result = _validator.TestValidate(productDto);
            result.ShouldHaveValidationErrorFor(p => p.Name).WithErrorMessage("O produto deve possuir um nome");
        }

        [Fact]
        public void ShouldHaveErrorWhenAvailableIsNull()
        {
            var productDto = new ProductDto { Name = "Test", Available = null, ProductCategory = ProductCategory.Sandwich, Value = 10 };
            var result = _validator.TestValidate(productDto);
            result.ShouldHaveValidationErrorFor(p => p.Available);
        }

        [Fact]
        public void ShouldHaveErrorWhenProductCategoryIsNull()
        {
            var productDto = new ProductDto { Name = "Test", Available = true, ProductCategory = null, Value = 10 };
            var result = _validator.TestValidate(productDto);
            result.ShouldHaveValidationErrorFor(p => p.ProductCategory);
        }

        [Fact]
        public void ShouldHaveErrorWhenValueIsNull()
        {
            var productDto = new ProductDto { Name = "Test", Available = true, ProductCategory = ProductCategory.Sandwich, Value = null };
            var result = _validator.TestValidate(productDto);
            result.ShouldHaveValidationErrorFor(p => p.Value);
        }

        [Fact]
        public void ShouldHaveErrorWhenValueIsNegative()
        {
            var productDto = new ProductDto { Name = "Test", Available = true, ProductCategory = ProductCategory.Sandwich, Value = -10 };
            var result = _validator.TestValidate(productDto);
            result.ShouldHaveValidationErrorFor(p => p.Value).WithErrorMessage("O produto deve ter o valor igual ou maior que 0");
        }

        [Fact]
        public void ShouldNotHaveErrorWhenProductDtoIsValid()
        {
            var productDto = new ProductDto { Name = "Test", Available = true, ProductCategory = ProductCategory.Sandwich, Value = 10 };
            var result = _validator.TestValidate(productDto);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
