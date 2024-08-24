using CookieAuthentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Principal;
using System.Xml.Linq;

namespace CookieAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        bool isAuthenticated = false;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private void CheckLoginStatus()
        {
            if (HttpContext.User.Identity != null)
            {
                isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
                if (isAuthenticated)
                {
                    ViewBag.UserName = HttpContext.User.Identity.Name;
                }
                ViewBag.IsLogin = isAuthenticated;
            }

            var location = User.Claims.FirstOrDefault(c => c.Type == "Location")?.Value;
            ViewData["Location"] = location;

        }

        public IActionResult Index()
        {
            CheckLoginStatus();

            return View();
        }

        [Authorize]
        public IActionResult Transient()
        {
            CheckLoginStatus();

            return View();
        }


        [Authorize]
        public IActionResult Privacy()
        {
            CheckLoginStatus();

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
