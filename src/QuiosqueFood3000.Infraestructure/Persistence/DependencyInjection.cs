using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuiosqueFood3000.Infraestructure.Repositories;
using QuiosqueFood3000.Infraestructure.Repositories.Interfaces;

namespace QuiosqueFood3000.Infraestructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderSolicitationRepository, OrderSolicitationRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        return services;

    }
}