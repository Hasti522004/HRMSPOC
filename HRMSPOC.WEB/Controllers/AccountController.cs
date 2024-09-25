﻿using HRMSPOC.WEB.DTOs;
using HRMSPOC.WEB.Models;
using HRMSPOC.WEB.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace HRMSPOC.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly HttpClient _httpClient;
        public AccountController(AuthService authService, HttpClient httpClient)
        {
            _authService = authService;
            _httpClient = httpClient;
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
                    // Decode the JWT token
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    // Retrieve roles from claims
                    var roles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                    var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

                    // Optionally, store roles in session or do something else with them
                    HttpContext.Session.SetString("UserRoles", string.Join(",", roles));

                    if (roles.Contains("SuperAdmin"))
                    {
                        return RedirectToAction("Index", "Organization");
                    }
                    else if (roles.Contains("Admin"))
                    {
                        var response = await _httpClient.GetAsync($"https://localhost:7095/api/UserOrganization/organization/{userId}");

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var organizationId = JsonConvert.DeserializeObject<OrganizationResponse>(content).OrganizationId;

                            // Set OrganizationId in session
                            HttpContext.Session.SetString("OrganizationId", organizationId.ToString());
                            return RedirectToAction("Index", "HR");
                        }
                        else
                        {
                            // Handle the case where the API call fails
                            ModelState.AddModelError(string.Empty, "Failed to retrieve organization ID.");
                            return View(); // Return to the current view with an error message
                        }
                    }
                    else if (roles.Contains("HR"))
                    {
                        HttpContext.Session.SetString("HrId", userId.ToString());
                        return RedirectToAction("Index", "Employee");
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

public class OrganizationResponse
{
    public Guid OrganizationId { get; set; }
}