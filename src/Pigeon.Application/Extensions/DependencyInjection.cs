using Microsoft.Extensions.DependencyInjection;
using Pigeon.Application.Services;

namespace Pigeon.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();

        return services;
    }
}