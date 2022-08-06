using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace Service;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IConfiguration _configuration;

    public UserService(ILogger<UserService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }


    public async Task<string> GetUserById(int id)
    {
        using HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(_configuration.GetSection("PlaceholderBaseAPI").Value);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var request = new HttpRequestMessage(HttpMethod.Get, "users/"+id.ToString());
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        try
        {
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return content;
            //var serializedResponse = JsonConvert.DeserializeObject<User>(content);
            //return serializedResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }

    }

    public async Task<string> GetUsers()
    {
        using HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(_configuration.GetSection("PlaceholderBaseAPI").Value);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var request = new HttpRequestMessage(HttpMethod.Get, "users");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        try
        {
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return content;
            //var serializedResponse = JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            //return serializedResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
