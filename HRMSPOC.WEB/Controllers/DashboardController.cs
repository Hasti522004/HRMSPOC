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
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.TryGetValue("OrganizationId", out var orgIdBytes))
            {
                var organizationId = new Guid(orgIdBytes);
               
                var userId = HttpContext.Session.GetString("UserId");
                var userRoles = HttpContext.Session.GetString("UserRoles");

                ViewBag.UserId = userId;
                ViewBag.UserRoles = userRoles;

                var users = await _dashboardService.GetUsersByOrganizationIdAsync(organizationId);
                return View(users);
            }

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
                    var createdById = Guid.Parse(createdByIdString);
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
                var users = await _dashboardService.GetUsersByOrganizationIdAsync(organizationId); 
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
        public async Task<IActionResult> DeleteConfirmed(Guid id) 
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
