using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplicationsgls.API.RateLimiting;

public static class RateLimitExtensions
{
    public static IServiceCollection AddApiRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions();
        services.AddMemoryCache();

        services.Configure<IpRateLimitOptions>(
            configuration.GetSection(RateLimitConfigurationSections.IpRateLimiting));

        services.Configure<IpRateLimitPolicies>(
            configuration.GetSection(RateLimitConfigurationSections.IpRateLimitPolicies));

        services.AddInMemoryRateLimiting();
        services.AddSingleton<IRateLimitConfiguration, AspNetCoreRateLimit.RateLimitConfiguration>();

        return services;
    }
}

