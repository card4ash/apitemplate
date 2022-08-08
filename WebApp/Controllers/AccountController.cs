using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security;
using System.Text;
using WebApp.Models;
using Infrastructure.DataService;
using Microsoft.AspNetCore.Authentication;
using WebApp.Helpers;
using Service.Token;

namespace WebApp.Controllers;

public class AccountController : Controller
{
    private readonly ILogger _logger;

    private readonly IConfiguration _configuration;
    private readonly IAppUserService _appUserService;
    private readonly ITokenService _tokenService;

    public AccountController(ILogger<AccountController> logger,
        IConfiguration configuration,
        IAppUserService appUserService,
        ITokenService tokenService)
    {
        _logger = logger;
        _configuration = configuration;
        _appUserService = appUserService;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        LoginViewModel loginViewModel = new LoginViewModel();
        ViewBag.ReturnUrl = returnUrl;
        return View(loginViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
    {
        if (!ModelState.IsValid) 
        {
            TempData["errorMsg"] = ModelStateErrorMessage.GetErrorMessage(ModelState);
            return View(model);
        }
        try
        {
            if (_appUserService.TryLogin(model.UserName,model.Password))
            {
                var token = await _tokenService.GetToken(model.UserName, model.Password);
                TempData["token"] = token.Replace("\"", "");
                var principal = CreatePrincipal(model.UserName);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (!string.IsNullOrEmpty(ReturnUrl) && !ReturnUrl.Trim().Equals("/"))
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["errorMsg"] = "Username or Password is incorrect.";
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("[Stack Trace]  [ " + ex.StackTrace + " ]  [Message] " + ex.Message);
            TempData["errorMsg"] = "Problem is there, contact with IT-HELPDESK.";
            return View(model);
        }
    }
    
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //[AllowAnonymous]
    public async Task<IActionResult> SignOut()
    {
        try
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message,ex);
        }

        return RedirectToAction("Login", "Account");
    }


    [HttpGet]
    [AllowAnonymous]
    public new IActionResult Unauthorized()
    {
        return View();
    }
    

    private ClaimsPrincipal CreatePrincipal(string username)
    {
        var roles = string.Join(",", _appUserService.GetRolesForUser(username).Select(r => r.ToString()));
        var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role,roles )
            };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        return principal;
    }


}
