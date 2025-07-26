using Microsoft.Extensions.DependencyInjection;
using QuiosqueFood3000.Api.Services;
using QuiosqueFood3000.Api.Services.Interfaces;
using QuiosqueFood3000.Application.Services;

namespace QuiosqueFood3000.Api.Infra;

public static class DependencyRegister
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderSolicitationService, OrderSolicitationService>();
        services.AddScoped<IProductService, ProductService>();
        
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}