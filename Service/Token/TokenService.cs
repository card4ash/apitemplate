using Entities.DataContract;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Service.Token;

public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private readonly IConfiguration _configuration;

    public TokenService(ILogger<TokenService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    public async Task<string> GetToken(string userName,string password)
    {
        using HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(_configuration.GetSection("TokenBaseUrl").Value);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var query = new Dictionary<string, string>
        {
            ["username"] = userName,
            ["password"]=password
        };
        try
        {
            var response = await client.GetAsync(QueryHelpers.AddQueryString("/api/v1/login", query));
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}

public interface ITokenService
{
    Task<string> GetToken(string userName, string password);
}
