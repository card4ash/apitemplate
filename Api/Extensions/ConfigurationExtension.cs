namespace Api.Extensions;

public static class ConfigurationExtension
{
    public static string GetSwaggerBase(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("SwaggerBase") ?? throw new Exception("SwaggerBase must be set");
    }
    
    public static string GetSecret(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("Secret") ?? throw new Exception("Secret must be set");
    }
    public static string GetRedisUrl(this IConfiguration configuration)
    {
        return configuration.GetValue<string>("RedisCacheUrl") ?? throw new Exception("Redis Cache Url must be set");
    }
    public static bool GetEnableCache(this IConfiguration configuration)
    {
        return configuration.GetValue<bool?>("enableCache") ?? throw new Exception("Redis Cache Url must be set");
    }
}