using Api.Middleware;

namespace Api.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseJwtAuthentication(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtAuthentication>();
    }
}