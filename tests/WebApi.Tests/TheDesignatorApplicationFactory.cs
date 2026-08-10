using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using TheDesignator.Domain.Security.PasswordHashing;
using TheDesignator.Infrastructure.DataAccess;
using WebApi.Tests.Resources;

namespace WebApi.Tests;

public class TheDesignatorApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public UserIdentityManager User1 { get; private set; }
    private readonly MsSqlContainer _msSqlContainer;

    public TheDesignatorApplicationFactory()
    {
        _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests")
            .ConfigureAppConfiguration((_, configuration) =>
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DbConnection"] = _msSqlContainer.GetConnectionString()
                };

                configuration.AddInMemoryCollection(parameters);
            });
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
        var scriptPath = Path.Combine(AppContext.BaseDirectory, "Scripts", "Users.sql");

        var script = await File.ReadAllTextAsync(scriptPath);
        var result = await _msSqlContainer.ExecScriptAsync(script);

        if (result.ExitCode != 0)
            throw new Exception($"Falha ao executar script: {result.Stderr}");

        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TheDesignatorContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var (user, password) = UserBuilder.Build();
        user.Password = passwordHasher.HashPassword(password);

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        User1 = new UserIdentityManager(user, password);
    }

    Task IAsyncLifetime.DisposeAsync() => _msSqlContainer.StopAsync();
}
