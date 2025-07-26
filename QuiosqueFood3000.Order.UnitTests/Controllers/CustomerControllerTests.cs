
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using QuiosqueFood3000.Controllers;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;

namespace QuiosqueFood3000.Order.UnitTests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomerController _customerController;

        public CustomerControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _customerController = new CustomerController(_customerServiceMock.Object);
        }

        [Fact]
        public async Task CustomerByCpf_WhenCustomerExists_ReturnsOkResultWithCustomerDto()
        {
            // Arrange
            var cpf = "11144477735";
            var customerDto = new CustomerDto { Cpf = cpf };
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(cpf)).ReturnsAsync(customerDto);

            // Act
            var result = await _customerController.CustomerByCpf(cpf);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerDto>(okResult.Value);
            Assert.Equal(cpf, returnValue.Cpf);
        }

        [Fact]
        public async Task CustomerByCpf_WhenCustomerDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var cpf = "11144477735";
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(cpf)).ReturnsAsync((CustomerDto)null);

            // Act
            var result = await _customerController.CustomerByCpf(cpf);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task RegisterCustomer_WhenCustomerDtoIsValid_ReturnsOkResultWithCustomerDto()
        {
            // Arrange
            var customerDto = new CustomerDto { Cpf = "11144477735", Name = "Test", Email = "test@test.com" };
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(customerDto.Cpf)).ReturnsAsync((CustomerDto)null);
            _customerServiceMock.Setup(x => x.RegisterCustomer(customerDto)).Returns(customerDto);

            // Act
            var result = await _customerController.RegisterCustomer(customerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerDto>(okResult.Value);
            Assert.Equal(customerDto.Cpf, returnValue.Cpf);
        }

        [Fact]
        public async Task RegisterCustomer_WhenCpfIsNull_ReturnsBadRequestResult()
        {
            // Arrange
            var customerDto = new CustomerDto { Cpf = null };

            // Act
            var result = await _customerController.RegisterCustomer(customerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("O CPF deve ser informado", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task RegisterCustomer_WhenCustomerAlreadyExists_ReturnsBadRequestResult()
        {
            // Arrange
            var customerDto = new CustomerDto { Cpf = "11144477735" };
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(customerDto.Cpf)).ReturnsAsync(customerDto);

            // Act
            var result = await _customerController.RegisterCustomer(customerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Cliente com o CPF: {customerDto.Cpf} já está cadastrado", badRequestResult.Value);
        }

        [Fact]
        public async Task RegisterCustomer_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var customerDto = new CustomerDto { Cpf = "11144477735" };
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(customerDto.Cpf)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _customerController.RegisterCustomer(customerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Service error", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveCustomer_WhenCustomerExists_ReturnsOkResult()
        {
            // Arrange
            var cpf = "11144477735";
            var customerDto = new CustomerDto { Cpf = cpf };
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(cpf)).ReturnsAsync(customerDto);

            // Act
            var result = await _customerController.RemoveCustomer(cpf);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Cliente removido com sucesso", okResult.Value);
            _customerServiceMock.Verify(x => x.RemoveCustomer(customerDto), Times.Once);
        }

        [Fact]
        public async Task RemoveCustomer_WhenCustomerDoesNotExist_ReturnsBadRequestResult()
        {
            // Arrange
            var cpf = "11144477735";
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(cpf)).ReturnsAsync((CustomerDto)null);

            // Act
            var result = await _customerController.RemoveCustomer(cpf);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Cliente com o CPF: {cpf} não está cadastrado", badRequestResult.Value);
        }

        [Fact]
        public async Task RemoveCustomer_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var cpf = "11144477735";
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(cpf)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _customerController.RemoveCustomer(cpf);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Service error", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateCustomer_WhenCustomerExists_ReturnsOkResultWithCustomerDto()
        {
            // Arrange
            var customerDto = new CustomerDto { Cpf = "11144477735", Name = "Updated Name", Email = "updated@test.com" };
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(customerDto.Cpf)).ReturnsAsync(customerDto);
            _customerServiceMock.Setup(x => x.UpdateCustomer(customerDto)).Returns(customerDto);

            // Act
            var result = await _customerController.UpdateCustomer(customerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerDto>(okResult.Value);
            Assert.Equal(customerDto.Cpf, returnValue.Cpf);
            Assert.Equal(customerDto.Name, returnValue.Name);
        }

        [Fact]
        public async Task UpdateCustomer_WhenCpfIsNull_ReturnsBadRequestResult()
        {
            // Arrange
            var customerDto = new CustomerDto { Cpf = null };

            // Act
            var result = await _customerController.UpdateCustomer(customerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("O CPF deve ser informado", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task UpdateCustomer_WhenCustomerDoesNotExist_ReturnsBadRequestResult()
        {
            // Arrange
            var customerDto = new CustomerDto { Cpf = "11144477735" };
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(customerDto.Cpf)).ReturnsAsync((CustomerDto)null);

            // Act
            var result = await _customerController.UpdateCustomer(customerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Cliente com o CPF: {customerDto.Cpf} não está cadastrado", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateCustomer_WhenServiceThrowsException_ReturnsBadRequestResult()
        {
            // Arrange
            var customerDto = new CustomerDto { Cpf = "11144477735" };
            _customerServiceMock.Setup(x => x.GetCustomerByCpf(customerDto.Cpf)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _customerController.UpdateCustomer(customerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Service error", badRequestResult.Value);
        }
    }
}
