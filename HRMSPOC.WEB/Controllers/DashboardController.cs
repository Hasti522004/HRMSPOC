using HRMSPOC.WEB.Services;
using HRMSPOC.WEB.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        // Display users
        public async Task<IActionResult> Index()
        {
            // Get the Organization ID from the session
            if (HttpContext.Session.TryGetValue("OrganizationId", out var orgIdBytes))
            {
                var organizationId = new Guid(orgIdBytes);

                // Call the service to get users by organization ID
                var users = await _dashboardService.GetUsersByOrganizationIdAsync(organizationId);
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
        public async Task<IActionResult> Create(UserViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                var success = await _dashboardService.CreateUserAsync(newUser);
                if (success)
                {
                    return RedirectToAction("Index");
                }
            }

            // If creation fails, return the view with validation errors
            return View(newUser);
        }

        // GET: Edit user view
        public async Task<IActionResult> Edit(string id)
        {
            var users = await _dashboardService.GetUsersByOrganizationIdAsync(Guid.NewGuid()); // You may want to replace Guid.NewGuid() with a valid organizationId if needed
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
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

        // POST: Delete user
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _dashboardService.DeleteUserAsync(id);
            if (success)
            {
                return RedirectToAction("Index");
            }

            // If deletion fails, return an error view or handle accordingly
            return View("Error");
        }
    }
}
