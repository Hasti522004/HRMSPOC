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
    }
}
