using Api.Extensions;
using Entities.DataContract;
using Entities.Enums;
using FluentValidation;
using Infrastructure.DataService;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Api.Controllers;


[ApiController]
public class UsersController : BaseController
{
    private readonly IUserService _userService;
    private readonly IValidator<User> _userValidator;
    private readonly IPostService _postService;

    public UsersController(
        ILogger<UsersController> logger,
        IAppUserService appUserService,
        IUserService userService,
        IValidator<User> userValidator,
        IPostService postService
        ) : base(logger, appUserService)
    {
        _userService = userService;
        _userValidator = userValidator;
        _postService = postService;
    }
    [Route("api/users")]
    [HttpGet]
    public Task<IActionResult> GetUsers()
    {
        return HandleRequest(
            async () =>
            {
                var users =await _userService.GetUsers();
                return Ok(users);
            }, Request, Role.None);
    }
    [Route("api/users/{id:int}")]
    [HttpGet]
    public Task<IActionResult> GetUsers(int id)
    {
        return HandleRequest(
             async () =>
            {
                var response =await _userService.GetUserById(id);
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
