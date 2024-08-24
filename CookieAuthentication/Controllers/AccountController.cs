#define LOCATION

using Microsoft.AspNetCore.Authentication.Cookies;
using CookieAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;



namespace CookieAuthentication.Controllers
{
    public class AccountController : Controller
    {

        //** you MUST type full url including 'location' and press enter key to open login form **
        //"https://localhost:7138/Account/Login?ReturnUrl=%2FHome%2FPrivacy&Location=Los%20Angeles" + enter key

        public IActionResult Login(string returnUrl = null, string location = null)
        {
            ViewData["ReturnUrl"] = returnUrl;      //callback action: "/Home/Privacy" or other action
            ViewData["Location"] = location;        //null because this url did not requested
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (userLogin.UserName == "admin" && userLogin.Password == "admin")
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, userLogin.UserName),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("Location", userLogin.Location ?? "Unknown")
                };
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));


                return LocalRedirect(userLogin.ReturnUrl);
                //return View();
            }
            else
            {
                return View();
            }

        }

#if noLOCATION
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (userLogin.UserName == "admin" && userLogin.Password == "admin")
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, userLogin.UserName),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return LocalRedirect(userLogin.ReturnUrl);
                //return View();
            }
            else 
            {
                return View();
            }

        }
#endif

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }

}
