using System.Security.Cryptography;
using Infrastructure.DataAccess;
using Infrastructure.DataService;
using JWT;
using JWT.Algorithms;
using JWT.Extensions.AspNetCore.Factories;
using Service;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDependencyInjection(this IServiceCollection services)
    {
        services.AddTransient<IJwtGenerator, JwtGenerator>();
        services.AddTransient<IAppUserService, AppUserService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IPostService, PostService>();
        services.AddSingleton(_ => ECDsa.Create());

        services.AddJwtDependencies();

        services.AddSingleton<IAppUserRepository, AppUserRepository>();
    }

    private static void AddJwtDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IIdentityFactory, DefaultIdentityFactory>();
        services.AddSingleton<ITicketFactory, DefaultTicketFactory>();
        services.AddSingleton<IJwtDecoder, JwtDecoder>();
        services.AddSingleton<IAlgorithmFactory, ECDSAAlgorithmFactory>();
    }
}