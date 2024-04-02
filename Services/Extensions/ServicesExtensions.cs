using Microsoft.Extensions.DependencyInjection;
using TaxCalculator.Services.Interfaces;

namespace TaxCalculator.Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITaxBandsService, TaxBandsService>();
        services.AddScoped<ITaxCalculator, TaxCalculator>();
        services.AddScoped<ITaxCalculatorService, TaxCalculatorService>();

        return services;
    }
}
