using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ITDIV_TICKET.Controllers;

public class LoginController : Controller
{   
    private readonly MovieService _movieSerivce;
    public LoginController(MovieService movieService)
    {
        _movieSerivce = movieService;
    }
   
    public async Task Register()
    {
        await HttpContext.ChallengeAsync("GoogleScheme",
            new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponseRegister")
            });
    }
    public async Task Login()
    {
        await HttpContext.ChallengeAsync("GoogleScheme",
            new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        
    }

    public async Task<IActionResult> GoogleResponseRegister()
    {
        var result = await HttpContext.AuthenticateAsync("GoogleScheme");

        if (result.Succeeded)
        {
            var email = result.Principal.FindFirstValue(ClaimTypes.Email); 
            var isSuccess = await _movieSerivce.DoRegister(email!,"none",email!,"none");
            if(!isSuccess) throw new Exception("Falied");
            return RedirectToAction("Login","WeCinema");
        }
        else
        {
            return RedirectToAction("Register", "WeCinema");  
        }

    }

    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync("GoogleScheme");

        if (result.Succeeded) 
        {
            var email = result.Principal.FindFirstValue(ClaimTypes.Email); 
            var checkEmail = await _movieSerivce.CheckEmail(email!);
            if(!checkEmail)
            {
                await HttpContext.SignOutAsync("CookieScheme");
                TempData["ErrorMessage"] = "Login Error";
                return RedirectToAction("Login","WeCinema");
            }  
            
            var claimList = new List<Claim>
            {
                new Claim(ClaimTypes.Email,email!)
            };

            var authProp = new AuthenticationProperties
            {
                IsPersistent = true, 
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) 
            };

            var claimIdentity = new ClaimsIdentity(claimList,"CookieScheme");

            await HttpContext.SignInAsync("CookieScheme", new ClaimsPrincipal(claimIdentity), authProp);
            
            return RedirectToAction("Index", "WeCinema");  
        }
        else
        {
            return RedirectToAction("Login","WeCinema");
        }

    }
    
}