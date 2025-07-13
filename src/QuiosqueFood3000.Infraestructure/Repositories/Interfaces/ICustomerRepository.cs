using QuiosqueFood3000.Domain.Entities;

namespace QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetCustomerbyCpf(string cpf);
    Customer RegisterCustomer(Customer customer);
    void RemoveCustomer(Customer customer);
    Customer UpdateCustomer(Customer customer);
}