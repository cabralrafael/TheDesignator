using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheDesignator.Domain.Repositories;
using TheDesignator.Domain.Repositories.User;
using TheDesignator.Domain.Security.PasswordHashing;
using TheDesignator.Infrastructure.DataAccess;
using TheDesignator.Infrastructure.DataAccess.Repositories;
using TheDesignator.Infrastructure.Security.PasswordHashing;

namespace TheDesignator.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUnityOfWork, UnityOfWork>();
        services.AddDbContext<TheDesignatorContext>(config =>
        {
            var connectionString = configuration.GetConnectionString("DbConnection");

            config.UseSqlServer(connectionString);
        });
    }
}
