using HRMSPOC.WEB.DTOs;
using HRMSPOC.WEB.Models;
using HRMSPOC.WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (ModelState.IsValid)
            {
                var token = await _authService.LoginAsync(loginDto);
                if (!string.IsNullOrEmpty(token))
                {
                    var redirectAction = await _authService.HandleLoginToken(token);
                    if (redirectAction == "SuperAdmin")
                    {
                        return RedirectToAction("Index", "Organization");
                    }
                    else if (redirectAction == "User")
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                ModelState.AddModelError("", "Login failed.");
            }
            return View(loginDto);
        }

        // POST: /Account/Logout
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWTToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
