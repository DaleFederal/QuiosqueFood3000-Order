using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QuiosqueFood3000.Domain.Entities;

namespace QuiosqueFood3000.Infraestructure.Persistence;

public class ApplicationDbContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public ApplicationDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var databaseHost = _configuration["DATABASE_HOST"];
        var databaseName = _configuration["DATABASE_NAME"];
        var databaseUser = _configuration["DATABASE_USER"];
        var databasePassword = _configuration["DATABASE_PASSWORD"];

        var connectionString = $"Host={databaseHost};Port=5432;Database={databaseName};Username={databaseUser};Password={databasePassword}";
        
        optionsBuilder.UseNpgsql(connectionString);
    }

    public DbSet<Customer> Customer { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderSolicitation> OrderSolicitation { get; set; }
    public DbSet<Product> Product { get; set; }
    
    public DbSet<OrderItem> OrderItem { get; set; }
}