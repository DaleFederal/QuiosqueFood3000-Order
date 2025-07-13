using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuiosqueFood3000.Api.Infra;
using QuiosqueFood3000.Infraestructure.Persistence;
using System.Text.Json.Serialization;

namespace QuiosqueFood3000.Api
{
    public static class AppStartup
    {
        public static WebApplicationBuilder CreateAppBuilder(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            builder.Services
                .AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            return builder;
        }

        public static WebApplication BuildApp(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}