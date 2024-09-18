using Banking.Application.Configuration;

namespace Banking.API.Extensions;

public static class ConfigurationExtensions
{
    public static void AddConfigurations(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
    }
}
