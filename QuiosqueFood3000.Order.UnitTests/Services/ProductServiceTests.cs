
using Moq;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Domain.Entities.Enums;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Order.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetProductById_WhenProductExists_ReturnsProductDto()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", Value = 10.0m, Available = true, ProductCategory = ProductCategory.Sandwich };
            _productRepositoryMock.Setup(x => x.GetProductbyId(productId)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id.ToString(), result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Value, result.Value);
        }

        [Fact]
        public async Task GetProductById_WhenProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            var productId = 1;
            _productRepositoryMock.Setup(x => x.GetProductbyId(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void RegisterProduct_WhenProductDtoIsValid_ReturnsProductDto()
        {
            // Arrange
            var productDto = new ProductDto { Name = "New Product", Available = true, ProductCategory = ProductCategory.Sandwich, Value = 15.0m };
            var product = new Product { Id = 1, Name = productDto.Name, Available = (bool)productDto.Available, ProductCategory = (ProductCategory)productDto.ProductCategory, Value = (decimal)productDto.Value };
            _productRepositoryMock.Setup(x => x.RegisterProduct(It.IsAny<Product>())).Returns(product);

            // Act
            var result = _productService.RegisterProduct(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id.ToString(), result.Id);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact]
        public void RegisterProduct_WhenProductDtoIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _productService.RegisterProduct(null));
        }

        [Fact]
        public void RegisterProduct_WhenProductDtoIsInvalid_ThrowsInvalidDataException()
        {
            // Arrange
            var productDto = new ProductDto { Name = "", Available = true, ProductCategory = ProductCategory.Sandwich, Value = 15.0m };

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => _productService.RegisterProduct(productDto));
        }

        [Fact]
        public void RemoveProduct_WhenProductDtoIsValid_CallsRemoveProduct()
        {
            // Arrange
            var productDto = new ProductDto { Id = "1" };

            // Act
            _productService.RemoveProduct(productDto);

            // Assert
            _productRepositoryMock.Verify(x => x.RemoveProduct(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void RemoveProduct_WhenProductDtoIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _productService.RemoveProduct(null));
        }

        [Fact]
        public void RemoveProduct_WhenProductIdIsNotProvided_ThrowsArgumentNullException()
        {
            // Arrange
            var productDto = new ProductDto();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _productService.RemoveProduct(productDto));
        }

        [Fact]
        public void UpdateProduct_WhenProductDtoIsValid_ReturnsProductDto()
        {
            // Arrange
            var productDto = new ProductDto { Id = "1", Name = "Updated Product", Available = false, ProductCategory = ProductCategory.Drink, Value = 20.0m };
            var product = new Product { Id = 1, Name = productDto.Name, Available = (bool)productDto.Available, ProductCategory = (ProductCategory)productDto.ProductCategory, Value = (decimal)productDto.Value };
            _productRepositoryMock.Setup(x => x.UpdateProduct(It.IsAny<Product>())).Returns(product);

            // Act
            var result = _productService.UpdateProduct(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id.ToString(), result.Id);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact]
        public void UpdateProduct_WhenProductDtoIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _productService.UpdateProduct(null));
        }

        [Fact]
        public void UpdateProduct_WhenProductDtoIsInvalid_ThrowsInvalidDataException()
        {
            // Arrange
            var productDto = new ProductDto { Id = "1", Name = "", Available = false, ProductCategory = ProductCategory.Drink, Value = 20.0m };

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => _productService.UpdateProduct(productDto));
        }

        [Fact]
        public async Task GetProductsByCategory_WhenProductsExist_ReturnsProductDtoList()
        {
            // Arrange
            var productCategory = ProductCategory.Sandwich;
            var products = new List<Product> { new Product { Id = 1, Name = "Test Product", Value = 10.0m, Available = true, ProductCategory = productCategory } };
            _productRepositoryMock.Setup(x => x.GetProductsByCategory(productCategory)).ReturnsAsync(products);

            // Act
            var result = await _productService.GetProductsByCategory(productCategory);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(products[0].Id.ToString(), result[0].Id);
        }

        [Fact]
        public async Task GetProductsByCategory_WhenProductsDoNotExist_ThrowsArgumentNullException()
        {
            // Arrange
            var productCategory = ProductCategory.Sandwich;
            _productRepositoryMock.Setup(x => x.GetProductsByCategory(productCategory)).ReturnsAsync((List<Product>)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.GetProductsByCategory(productCategory));
        }
    }
}
