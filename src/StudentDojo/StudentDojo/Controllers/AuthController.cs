using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentDojo.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("login")]
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(
            GoogleDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = "/" });
        }

        [HttpGet("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Redirect("/");
        }
    }
}
