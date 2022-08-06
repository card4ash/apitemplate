using Infrastructure.DataService;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class AuthController : BaseController
{
    private readonly IJwtGenerator _jwtGenerator;

    public AuthController(ILogger<AuthController> logger, IAppUserService userService, IJwtGenerator jwtGenerator) : base(logger, userService)
    {
        _jwtGenerator = jwtGenerator;
    }

    [HttpGet("/api/v1/login")]
    public IActionResult Login(string username, string password)
    {
        return UserService.TryLogin(username, password) ?
            Ok(_jwtGenerator.GetJwt(username)) :
            Unauthorized();
    }
}