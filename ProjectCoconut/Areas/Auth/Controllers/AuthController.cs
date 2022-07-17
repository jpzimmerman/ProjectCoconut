using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ProjectCoconut.Areas.Auth.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public string Index() => "yay";
        
        [HttpGet]
        public IActionResult Login(string returnUrl = "/", string provider = "Identity.Application")
        {
            return Challenge(new AuthenticationProperties()
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(5.0f),
                RedirectUri = returnUrl,
                AllowRefresh = false,
            }, provider);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            SignOut(new AuthenticationProperties(), CookieAuthenticationDefaults.AuthenticationScheme,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );
            await HttpContext.SignOutAsync();
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }

            return RedirectToAction("Index", "/");
        }
    }
}
