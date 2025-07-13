using QuiosqueFood3000.Api.DTOs;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Api.Validators;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Application.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<CustomerDto?> GetCustomerByCpf(string cpf)
    {
        var customer = await customerRepository.GetCustomerbyCpf(cpf);

        return customer == null
            ? null
            : new CustomerDto()
            {
                Id = customer.Id.ToString(),
                Cpf = cpf,
                Name = customer.Name,
                Email = customer.Email,
            };
    }
    public CustomerDto RegisterCustomer(CustomerDto customerDto)
    {
        CustomerDtoValidator customerDtoValidator = new CustomerDtoValidator();
        var resultCustomerDto = customerDtoValidator.Validate(customerDto);

        if (!resultCustomerDto.IsValid)
        {
            throw new InvalidDataException(resultCustomerDto.ToString());
        }

        Customer customer = new Customer()
        {
            Cpf = customerDto.Cpf,
            Name = customerDto.Name,
            Email = customerDto.Email
        };
        CustomerValidator customerValidator = new CustomerValidator();
        var resultCustomer = customerValidator.Validate(customer);

        if (!resultCustomer.IsValid)
        {
            throw new InvalidDataException(resultCustomer.ToString());
        }
        customerRepository.RegisterCustomer(customer);
        customerDto = new CustomerDto()
        {
            Id = customer.Id.ToString(),
            Cpf = customer.Cpf,
            Name = customer.Name,
            Email = customer.Email
        };
        return customerDto;
    }
    public void RemoveCustomer(CustomerDto customerDto)
    {
        if (string.IsNullOrWhiteSpace(customerDto.Id))
        {
            throw new ArgumentNullException("O Id do cliente deve ser informado");
        }
        Customer customer = new Customer()
        {
            Id = int.Parse(customerDto.Id),
            Cpf = customerDto.Cpf,
            Name = customerDto.Name,
            Email = customerDto.Email
        };
        customerRepository.RemoveCustomer(customer);
    }
    public CustomerDto UpdateCustomer(CustomerDto customerDto)
    {

        if (customerDto == null)
        {
            throw new ArgumentNullException("O cliente não pode ser nulo");
        }

        if (string.IsNullOrWhiteSpace(customerDto.Id))
        {
            throw new ArgumentNullException("O Id do cliente deve ser informado");
        }
        CustomerDtoValidator customerDtoValidator = new CustomerDtoValidator();
        var resultCustomerDto = customerDtoValidator.Validate(customerDto);

        if (!resultCustomerDto.IsValid)
        {
            throw new InvalidDataException(resultCustomerDto.ToString());
        }

        Customer customer = new Customer()
        {
            Id = int.Parse(customerDto.Id),
            Cpf = customerDto.Cpf,
            Name = customerDto.Name,
            Email = customerDto.Email
        };
        CustomerValidator customerValidator = new CustomerValidator();
        var resultCustomer = customerValidator.Validate(customer);

        if (!resultCustomer.IsValid)
        {
            throw new InvalidDataException(resultCustomer.ToString());
        }
        customer = customerRepository.UpdateCustomer(customer);
        customerDto = new CustomerDto()
        {
            Id = customer.Id.ToString(),
            Cpf = customer.Cpf,
            Name = customer.Name,
            Email = customer.Email
        };
        return customerDto;
    }
}