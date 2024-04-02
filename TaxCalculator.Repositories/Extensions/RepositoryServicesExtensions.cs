using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaxCalculator.Repositories.DBContext;
using TaxCalculator.Repositories.Interfaces;
using TaxCalculator.Repositories.Mapping;

namespace TaxCalculator.Repositories.Extensions;
using Mapster;
using MapsterMapper;

public static class RepositoryServicesExtensions
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));
        
        services.AddDbContext<UserDbContext>(options => options.UseSqlServer(connectionString));
        services.AddDbContext<CountryTaxBandsDbContext>(options => options.UseSqlServer(connectionString));
        
        services.AddScoped<ICountryTaxBandsRepository, CountryTaxBandsRepository>();

        return services;
    }

    public static IServiceCollection AddRepositoryMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MappingConfiguration).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }

}
