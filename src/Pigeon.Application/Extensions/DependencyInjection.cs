using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pigeon.Application.Middlewares;
using Pigeon.Application.Services;

namespace Pigeon.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IChatService, ChatService>();

        return services;
    }

    public static IApplicationBuilder UseApplication(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<ExceptionHandlingMiddleware>();

        return applicationBuilder;
    }
}