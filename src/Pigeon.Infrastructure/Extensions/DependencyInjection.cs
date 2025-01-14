using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pigeon.Domain.Interfaces.Repositories;
using Pigeon.Infrastructure.Repositories;

namespace Pigeon.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddTransient<IUserRepository, UserRepository>();

        return services;
    }
}