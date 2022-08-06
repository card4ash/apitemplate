using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace Service;

public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;
    private readonly IConfiguration _configuration;

    public PostService(ILogger<PostService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }


    public async Task<string> GetPostsByUserId(int userId)
    {
        using HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(_configuration.GetSection("PlaceholderBaseAPI").Value);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var query = new Dictionary<string, string>
        {
            ["userId"] = userId.ToString()
        };
        try
        {
            var response = await client.GetAsync(QueryHelpers.AddQueryString("/posts", query));
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
}
