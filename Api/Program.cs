using Api.Extensions;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using FluentValidation.AspNetCore;
using JWT.Extensions.AspNetCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Api.Validation;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Hello, world!");
try
{

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddDependencyInjection();

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = HeaderNames.Authorization,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                $"JWT Authorization header using the Bearer scheme. {Environment.NewLine}" +
                "Enter \"Bearer 'JWT'\" in the text input below."
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    });

    builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("https://localhost:7231").AllowAnyMethod().AllowAnyHeader();
    }));

    builder.Services.AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining<UserValidator>());

    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore
    };

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
    })
        .AddJwt(options =>
        {
            options.Keys = new[] { builder.Configuration.GetSecret() };
            //options.Keys = null;
            options.VerifySignature = true;
        });

    builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = builder.Configuration.GetRedisUrl(); });

    var app = builder.Build();
    app.UseSerilogRequestLogging();

    //app.UseAuthentication();
    //app.UseJwtAuthentication();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = "Placeholder Gateway - Swagger";
            c.RoutePrefix = string.Empty;
            //c.SwaggerEndpoint($"{builder.Configuration.GetSwaggerBase()}/swagger/v1/swagger.json", "v1");
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            c.RoutePrefix = "swagger";
        });
    }
    app.UseCors("corsapp");
    app.UseHttpsRedirection();

    //app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

