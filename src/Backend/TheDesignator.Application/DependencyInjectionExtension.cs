using Microsoft.Extensions.DependencyInjection;
using TheDesignator.Application.UseCases.User.Register;

namespace TheDesignator.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
    }
}
