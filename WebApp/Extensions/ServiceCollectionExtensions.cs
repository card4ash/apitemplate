using Infrastructure.DataAccess;
using Infrastructure.DataService;
using Service.Token;

namespace WebApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddTransient<IAppUserService, AppUserService>();
        services.AddSingleton<IAppUserRepository, AppUserRepository>();
        services.AddSingleton<ITokenService, TokenService>();
    }
}