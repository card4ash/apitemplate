using Api.Extensions;
using Entities.DataContract;
using Entities.Enums;
using FluentValidation;
using Infrastructure.DataService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Service;
using System.Text;

namespace Api.Controllers;


[ApiController]
public class UsersController : BaseController
{
    private readonly IUserService _userService;
    private readonly IValidator<User> _userValidator;
    private readonly IPostService _postService;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    public UsersController(
        ILogger<UsersController> logger,
        IAppUserService appUserService,
        IUserService userService,
        IValidator<User> userValidator,
        IPostService postService,
        IDistributedCache cache,
        IConfiguration configuration
        ) : base(logger, appUserService)
    {
        _userService = userService;
        _userValidator = userValidator;
        _postService = postService;
        _cache = cache;
        _configuration = configuration;
    }
    [Route("api/users")]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return await HandleRequest(
            async () =>
            {
                return await GetAllUser();
            }, Request, Role.None);
    }

    private async Task<IActionResult> GetAllUser()
    {
        var enableCache = _configuration.GetEnableCache();
        if (!enableCache)
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }
        string cacheKey = "users";
        byte[] cachedData = await _cache.GetAsync(cacheKey);
        if (cachedData != null)
        {
            var cachedDataString = Encoding.UTF8.GetString(cachedData);
            return Ok(cachedDataString);
        }
        else
        {
            var users = await _userService.GetUsers();
            var dataToCache = Encoding.UTF8.GetBytes(users);

            // Setting up the cache options
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(3));

            // Add the data into the cache
            await _cache.SetAsync(cacheKey, dataToCache, options);
            return Ok(users);
        }
    }

    [Route("api/users/{id:int}")]
    [HttpGet]
    public Task<IActionResult> GetUsers(int id)
    {
        return HandleRequest(
             async () =>
            {
                var response = await _userService.GetUserById(id);
                return Ok(response);
            }, Request, Role.None);
    }
    [Route("api/posts")]
    [HttpGet]
    public Task<IActionResult> GetPostsByUserId([FromQuery] int userid)
    {
        return HandleRequest(
             async () =>
             {
                 var response = await _postService.GetPostsByUserId(userid);
                 return Ok(response);
             }, Request, Role.None);
    }
    [Route("api/users/{userid:int}/posts")]
    [HttpGet]
    public Task<IActionResult> GetUsersPostsByUserId(int userid)
    {
        return HandleRequest(
             async () =>
             {
                 var response = await _postService.GetPostsByUserId(userid);
                 return Ok(response);
             }, Request, Role.None);
    }

}
