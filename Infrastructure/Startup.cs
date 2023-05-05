using Application.Abstraction;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<ProductDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DbConnection")));
        services.AddScoped<IApplicationDbContext, ProductDbContext>();
        return services;
    }

}