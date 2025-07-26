using Microsoft.EntityFrameworkCore;
using QuiosqueFood3000.Domain.Entities;
using QuiosqueFood3000.Infraestructure.Persistence;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Infraestructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetCustomerbyCpf(string cpf) => await context.Customer.FirstOrDefaultAsync(c => c.Cpf == cpf);
    public Customer RegisterCustomer(Customer customer)

    {
        context.Customer.Add(customer);
        context.SaveChanges();
        return customer;
    }
    public void RemoveCustomer(Customer customer)
    {
        var existingCustomer = context.Customer.Find(customer.Id);
        if (existingCustomer != null)
        {
            context.Entry(existingCustomer).State = EntityState.Detached;
        }
        context.Customer.Remove(customer);
        context.SaveChanges();
    }
    public Customer UpdateCustomer(Customer customer)
    {
        var existingCustomer = context.Customer.Find(customer.Id);
        if (existingCustomer is null)
        {
            throw new InvalidOperationException("Cliente não encontrado.");
        }

        context.Entry(existingCustomer).State = EntityState.Detached;
        context.Customer.Update(customer);
        context.SaveChanges();
        return customer;
    }
}
