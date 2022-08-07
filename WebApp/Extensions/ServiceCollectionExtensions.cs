using Infrastructure.DataAccess;
using Infrastructure.DataService;

namespace WebApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddTransient<IAppUserService, AppUserService>();
        services.AddSingleton<IAppUserRepository, AppUserRepository>();
    }
}