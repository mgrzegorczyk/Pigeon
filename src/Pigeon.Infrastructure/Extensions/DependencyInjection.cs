using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pigeon.Domain.Interfaces.Repositories;
using Pigeon.Infrastructure.Options;
using Pigeon.Infrastructure.Repositories;

namespace Pigeon.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IChatRepository, ChatRepository>();
        services.AddAuth(connectionString, configuration);
        services.AddOptions(configuration);

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseAuthentication();
        applicationBuilder.UseAuthorization();
        return applicationBuilder;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettingsOptions>(configuration.GetSection(nameof(JwtSettingsOptions)));
        
        services.AddOptions<KafkaOptions>()
            .Bind(configuration.GetSection(nameof(KafkaOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddOptions<MessageHubOptions>()
            .Bind(configuration.GetSection(nameof(MessageHubOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        return services;
    }
}