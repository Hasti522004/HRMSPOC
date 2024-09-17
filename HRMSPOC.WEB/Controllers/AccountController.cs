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
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                if (registerDTO.Password != registerDTO.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                    return View(registerDTO);
                }

                var token = await _authService.RegisterAsync(registerDTO);
                if (!string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Account");
                }
                ModelState.AddModelError("", "Registration Failed. Please try again.");
            }
            return View(registerDTO);
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
                    // Store token (e.g., in session, cookies)
                    HttpContext.Session.SetString("JWTToken", token);

                    return RedirectToAction("Index", "Home");
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
