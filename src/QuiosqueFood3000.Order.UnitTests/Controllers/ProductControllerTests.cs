
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using QuiosqueFood3000.Controllers;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Domain.Entities.Enums;

namespace QuiosqueFood3000.Order.UnitTests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductController _productController;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _productController = new ProductController(_productServiceMock.Object);
        }

        [Fact]
        public async Task ProductById_WhenProductExists_ReturnsOkResultWithProductDto()
        {
            // Arrange
            var productId = 1;
            var productDto = new ProductDto { Id = productId.ToString() };
            _productServiceMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(productDto);

            // Act
            var result = await _productController.ProductById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(productId.ToString(), returnValue.Id);
        }

        [Fact]
        public async Task ProductById_WhenProductDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var productId = 1;
            _productServiceMock.Setup(x => x.GetProductById(productId)).ReturnsAsync((ProductDto)null);

            // Act
            var result = await _productController.ProductById(productId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }        

        [Fact]
        public void RegisterProduct_WhenProductDtoIsValid_ReturnsOkResultWithProductDto()
        {
            // Arrange
            var productDto = new ProductDto { Name = "Test Product", Available = true, ProductCategory = ProductCategory.Sandwich, Value = 10 };
            _productServiceMock.Setup(x => x.RegisterProduct(productDto)).Returns(productDto);

            // Act
            var result = _productController.RegisterProduct(productDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(productDto.Name, returnValue.Name);
        }

        [Fact]
        public void RegisterProduct_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var productDto = new ProductDto { Name = "Test Product", Available = true, ProductCategory = ProductCategory.Sandwich, Value = 10 };
            _productServiceMock.Setup(x => x.RegisterProduct(productDto)).Throws(new Exception("Service error"));

            // Act
            var result = _productController.RegisterProduct(productDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Service error", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task RemoveProduct_WhenProductExists_ReturnsOkResult()
        {
            // Arrange
            var productId = 1;
            var productDto = new ProductDto { Id = productId.ToString() };
            _productServiceMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(productDto);

            // Act
            var result = await _productController.RemoveProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Produto removido com sucesso", okResult.Value);
            _productServiceMock.Verify(x => x.RemoveProduct(productDto), Times.Once);
        }

        [Fact]
        public async Task RemoveProduct_WhenProductDoesNotExist_ReturnsBadRequestResult()
        {
            // Arrange
            var productId = 1;
            _productServiceMock.Setup(x => x.GetProductById(productId)).ReturnsAsync((ProductDto)null);

            // Act
            var result = await _productController.RemoveProduct(productId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Produto com o Id: {productId} não está cadastrado", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveProduct_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var productId = 1;
            var productDto = new ProductDto { Id = productId.ToString() };
            _productServiceMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(productDto);
            _productServiceMock.Setup(x => x.RemoveProduct(productDto)).Throws(new Exception("Service error"));

            // Act
            var result = await _productController.RemoveProduct(productId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Service error", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task UpdateProduct_WhenProductDtoIsValid_ReturnsOkResultWithProductDto()
        {
            // Arrange
            var productDto = new ProductDto { Id = "1", Name = "Updated Product", Available = false, ProductCategory = ProductCategory.Drink, Value = 20.0m };
            _productServiceMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync(productDto);
            _productServiceMock.Setup(x => x.UpdateProduct(productDto)).Returns(productDto);

            // Act
            var result = await _productController.UpdateProduct(productDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(productDto.Id, returnValue.Id);
            Assert.Equal(productDto.Name, returnValue.Name);
        }

        [Fact]
        public async Task UpdateProduct_WhenProductIdIsNull_ReturnsBadRequestResult()
        {
            // Arrange
            var productDto = new ProductDto { Id = null };

            // Act
            var result = await _productController.UpdateProduct(productDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Id do produto não pode ser nulo ou vazio", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_WhenProductDoesNotExist_ReturnsBadRequestResult()
        {
            // Arrange
            var productDto = new ProductDto { Id = "1" };
            _productServiceMock.Setup(x => x.GetProductById(It.IsAny<int>())).ReturnsAsync((ProductDto)null);

            // Act
            var result = await _productController.UpdateProduct(productDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Produto com o Id: {productDto.Id} não está cadastrado", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var productDto = new ProductDto { Id = "1" };
            _productServiceMock.Setup(x => x.GetProductById(It.IsAny<int>())).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _productController.UpdateProduct(productDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Service error", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task GetProductsByCategory_WhenProductsExist_ReturnsOkResultWithProductDtoList()
        {
            // Arrange
            var productCategory = ProductCategory.Sandwich;
            var productsDto = new List<ProductDto> { new ProductDto { Id = "1", ProductCategory = productCategory } };
            _productServiceMock.Setup(x => x.GetProductsByCategory(productCategory)).ReturnsAsync(productsDto);

            // Act
            var result = await _productController.GetProductsByCategory(productCategory);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetProductsByCategory_WhenProductsDoNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var productCategory = ProductCategory.Sandwich;
            _productServiceMock.Setup(x => x.GetProductsByCategory(productCategory)).ReturnsAsync((List<ProductDto>)null);

            // Act
            var result = await _productController.GetProductsByCategory(productCategory);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Não foram encontrados produtos com a categoria: {productCategory}", notFoundResult.Value);
        }        
    }
}
