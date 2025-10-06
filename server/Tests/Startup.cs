using efscaffold;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace Tests;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        
        Program.ConfigureServices(services, new ConfigurationBuilder().Build());
        services.RemoveAll(typeof(MyDbContext));
        services.AddScoped<MyDbContext>(factory =>
        {
            var container = new PostgreSqlBuilder().Build();
            container.StartAsync().GetAwaiter().GetResult();
            var connectionString = container.GetConnectionString();
            var options = new DbContextOptionsBuilder<MyDbContext>().UseNpgsql(connectionString).Options;
            var context = new MyDbContext(options);
            context.Database.EnsureCreated();
            return context;
        });
        services.RemoveAll(typeof(ISeeder));
        services.AddScoped<ISeeder, Seeder>();
        
    }
}