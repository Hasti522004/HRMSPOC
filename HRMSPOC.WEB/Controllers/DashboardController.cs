using HRMSPOC.WEB.Models;
using HRMSPOC.WEB.Services;
using HRMSPOC.WEB.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HRMSPOC.WEB.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public async Task<IActionResult> SetOrganizationId(string userId)
        {
            await _dashboardService.SetOrganizationAndCreatedById(userId);
            return RedirectToAction("Index", "Dashboard");
        }
        // Display users
        public async Task<IActionResult> Index()
        {
            // Get the Organization ID from the session
            if (HttpContext.Session.TryGetValue("OrganizationId", out var orgIdBytes))
            {
                var organizationId = new Guid(orgIdBytes);
               
                var userId = HttpContext.Session.GetString("UserId");
                var userRoles = HttpContext.Session.GetString("UserRoles");

                // Set ViewBag properties
                ViewBag.UserId = userId;
                ViewBag.UserRoles = userRoles;

                //// Call the service to get users by organization ID
                var users = await _dashboardService.GetUsersByOrganizationIdAsync(organizationId);
                //var adminUser = users.FirstOrDefault(u => u.RoleName == "Admin");

                //if (adminUser != null)
                //{
                //    // Store the Admin user's Id in the session as CreatedById
                //    HttpContext.Session.SetString("CreatedById", adminUser.Id);
                //}
                return View(users);
            }

            // If no users found or session organization ID is invalid
            return View(new List<UserViewModel>());
        }

        // GET: Create new user view
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create new user
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUserViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.TryGetValue("CreatedById", out var createdByIdBytes))
                {
                    var createdByIdString = System.Text.Encoding.UTF8.GetString(createdByIdBytes);
                    var createdById = Guid.Parse(createdByIdString); // Convert the string back to Guid
                    newUser.CreatedBy = createdById;

                    string role = newUser.RoleName;
                    var success = await _dashboardService.CreateUserAsync(newUser, role);
                    if (success)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(newUser);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (HttpContext.Session.TryGetValue("OrganizationId", out var orgIdBytes))
            {
                var organizationId = new Guid(orgIdBytes);
                var users = await _dashboardService.GetUsersByOrganizationIdAsync(organizationId); // You may want to replace Guid.NewGuid() with a valid organizationId if needed
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            return NotFound();
        }

        // POST: Edit user
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel updatedUser)
        {
            if (ModelState.IsValid)
            {
                var success = await _dashboardService.EditUserAsync(updatedUser);
                if (success)
                {
                    return RedirectToAction("Index");
                }
            }

            // If update fails, return the view with validation errors
            return View(updatedUser);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (HttpContext.Session.TryGetValue("OrganizationId", out var orgIdBytes))
            {
                var organizationId = new Guid(orgIdBytes);

                var users = await _dashboardService.GetUsersByOrganizationIdAsync(organizationId);
                var user = users.FirstOrDefault(u => u.Id == id.ToString());
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id)  // Update this method name to avoid confusion
        {
            var success = await _dashboardService.DeleteUserAsync(id);
            if (success)
            {
                return RedirectToAction("Index");
            }

            return View("Error");
        }
    }
}
