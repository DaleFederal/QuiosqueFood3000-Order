using QuiosqueFood3000.Api.DTOs;

namespace QuiosqueFood3000.Api.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> GetCustomerByCpf(string cpf);
    CustomerDto RegisterCustomer(CustomerDto customerDto);
    void RemoveCustomer(CustomerDto customerDto);
    CustomerDto UpdateCustomer(CustomerDto customerDto);
}