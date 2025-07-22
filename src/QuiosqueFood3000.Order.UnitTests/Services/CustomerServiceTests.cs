
using Moq;
using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Application.Services;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Order.UnitTests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_customerRepositoryMock.Object);
        }

        [Fact]
        public async Task GetCustomerByCpf_WhenCustomerExists_ReturnsCustomerDto()
        {
            // Arrange
            var cpf = "12345678901";
            var customer = new Customer { Id = 1, Name = "Test", Cpf = cpf, Email = "test@test.com" };
            _customerRepositoryMock.Setup(x => x.GetCustomerbyCpf(cpf)).ReturnsAsync(customer);

            // Act
            var result = await _customerService.GetCustomerByCpf(cpf);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Id.ToString(), result.Id);
            Assert.Equal(customer.Name, result.Name);
            Assert.Equal(customer.Cpf, result.Cpf);
            Assert.Equal(customer.Email, result.Email);
        }

        [Fact]
        public async Task GetCustomerByCpf_WhenCustomerDoesNotExist_ReturnsNull()
        {
            // Arrange
            var cpf = "12345678901";
            _customerRepositoryMock.Setup(x => x.GetCustomerbyCpf(cpf)).ReturnsAsync((Customer)null);

            // Act
            var result = await _customerService.GetCustomerByCpf(cpf);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void RegisterCustomer_WhenCustomerDtoIsValid_ReturnsCustomerDto()
        {
            // Arrange
            var customerDto = new CustomerDto { Name = "Test", Cpf = "11144477735", Email = "test@test.com" };
            var customer = new Customer { Id = 1, Name = customerDto.Name, Cpf = customerDto.Cpf, Email = customerDto.Email };
            _customerRepositoryMock.Setup(x => x.RegisterCustomer(It.IsAny<Customer>())).Returns(customer);

            // Act
            var result = _customerService.RegisterCustomer(customerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Id.ToString(), result.Id);
            Assert.Equal(customer.Name, result.Name);
            Assert.Equal(customer.Cpf, result.Cpf);
            Assert.Equal(customer.Email, result.Email);
        }

        [Fact]
        public void RegisterCustomer_WhenCustomerDtoIsInvalid_ThrowsInvalidDataException()
        {
            // Arrange
            var customerDto = new CustomerDto { Name = "Test", Cpf = "123", Email = "test@test.com" };

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => _customerService.RegisterCustomer(customerDto));
        }

        [Fact]
        public void RemoveCustomer_WhenCustomerIdIsProvided_CallsRemoveCustomer()
        {
            // Arrange
            var customerDto = new CustomerDto { Id = "1" };

            // Act
            _customerService.RemoveCustomer(customerDto);

            // Assert
            _customerRepositoryMock.Verify(x => x.RemoveCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public void RemoveCustomer_WhenCustomerIdIsNotProvided_ThrowsArgumentNullException()
        {
            // Arrange
            var customerDto = new CustomerDto();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _customerService.RemoveCustomer(customerDto));
        }

        [Fact]
        public void UpdateCustomer_WhenCustomerDtoIsValid_ReturnsCustomerDto()
        {
            // Arrange
            var customerDto = new CustomerDto { Id = "1", Name = "Test", Cpf = "11144477735", Email = "test@test.com" };
            var customer = new Customer { Id = 1, Name = customerDto.Name, Cpf = customerDto.Cpf, Email = customerDto.Email };
            _customerRepositoryMock.Setup(x => x.UpdateCustomer(It.IsAny<Customer>())).Returns(customer);

            // Act
            var result = _customerService.UpdateCustomer(customerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Id.ToString(), result.Id);
            Assert.Equal(customer.Name, result.Name);
            Assert.Equal(customer.Cpf, result.Cpf);
            Assert.Equal(customer.Email, result.Email);
        }

        [Fact]
        public void UpdateCustomer_WhenCustomerDtoIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _customerService.UpdateCustomer(null));
        }

        [Fact]
        public void UpdateCustomer_WhenCustomerIdIsNotProvided_ThrowsArgumentNullException()
        {
            // Arrange
            var customerDto = new CustomerDto { Name = "Test", Cpf = "11144477735", Email = "test@test.com" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _customerService.UpdateCustomer(customerDto));
        }

        [Fact]
        public void UpdateCustomer_WhenCustomerDtoIsInvalid_ThrowsInvalidDataException()
        {
            // Arrange
            var customerDto = new CustomerDto { Id = "1", Name = "Test", Cpf = "123", Email = "test@test.com" };

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => _customerService.UpdateCustomer(customerDto));
        }
    }
}
