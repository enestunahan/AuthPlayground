using AuthPlayground.Application.Common.Repositories;
using AuthPlayground.Application.Common.Repositories.Books;
using AuthPlayground.Persistence.Contexts;
using AuthPlayground.Persistence.Repositories;
using AuthPlayground.Persistence.Repositories.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthPlayground.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthPlaygroundDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is required.");

            options.UseNpgsql(connectionString);
        });

        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
        services.AddScoped<IBookReadRepository, BookReadRepository>();
        services.AddScoped<IBookWriteRepository, BookWriteRepository>();

        return services;
    }
}
