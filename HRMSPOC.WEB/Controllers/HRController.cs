using HRMSPOC.WEB.Models;
using HRMSPOC.WEB.DTOs;
using HRMSPOC.WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.WEB.Controllers
{
    public class HRController : Controller
    {
        private readonly UserService _userService;

        public HRController(UserService userService)
        {
            _userService = userService;
        }

        // GET: Organization/SetOrganizationId/{organizationId}
        public IActionResult SetOrganizationId(Guid organizationId)
        {
            HttpContext.Session.SetString("OrganizationId", organizationId.ToString());
            return RedirectToAction("Index", "HR"); // Redirect to User Index
        }

        // Private method to retrieve OrganizationId from session
        private Guid? GetOrganizationIdFromSession()
        {
            var organizationIdString = HttpContext.Session.GetString("OrganizationId");
            if (Guid.TryParse(organizationIdString, out var organizationId))
            {
                return organizationId;
            }
            return null; // Return null if the ID is not found or not valid
        }

        // GET: User/Index
        public async Task<IActionResult> Index()
        {
            var organizationId = GetOrganizationIdFromSession();

            if (organizationId.HasValue)
            {
                // Fetch users for the specific organization
                var users = await _userService.GetUsersByOrganizationIdAsync(organizationId.Value);
                ViewBag.OrganizationId = organizationId.Value; // Pass the organization ID to the view
                return View(users);
            }
            return RedirectToAction("Index", "HR"); // Redirect if no organization ID is found
        }

        // GET: User/Create
        public IActionResult Create()
        {
            var organizationId = GetOrganizationIdFromSession();
            if (organizationId.HasValue)
            {
                ViewBag.OrganizationId = organizationId.Value;
                return View();
            }
            return RedirectToAction("Index", "HR"); // Redirect if no organization ID is found
        }

        // POST: User/Create
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUserViewModel user)
        {
            var organizationId = GetOrganizationIdFromSession();
            if (organizationId.HasValue)
            {
                // Set CreatedBy field
                user.CreatedBy = organizationId.Value;

                if (ModelState.IsValid)
                {
                    var success = await _userService.CreateUserAsync(user);
                    if (success)
                    {
                        return RedirectToAction("Index", "HR"); // Redirect to Index
                    }
                }
            }
            return View(user);
        }

        // GET: User/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var userDto = await _userService.GetUserByIdAsync(id);
            if (userDto != null)
            {
                // Include the ID in ViewBag for the POST method
                ViewBag.UserId = id; // Store the UserId to use in POST action
                return View(userDto); // Return the DTO to the view
            }
            return NotFound();
        }

        // POST: User/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                // Ensure the userId is correctly passed
                if (string.IsNullOrEmpty(userDto.Id))
                {
                    return BadRequest("User ID cannot be null or empty.");
                }

                var success = await _userService.UpdateUserAsync(userDto);
                if (success)
                {
                    return RedirectToAction("Index", "HR"); // Redirect to Index
                }
            }
            return View(userDto); // Return the DTO back to the view if validation fails
        }

        // GET: User/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id.ToString());
            if (user != null)
            {
                return View(user);
            }
            return NotFound();
        }

        // POST: User/DeleteConfirmed/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (success)
            {
                var organizationId = GetOrganizationIdFromSession(); // Use the session to get the OrganizationId
                if (organizationId.HasValue)
                {
                    return RedirectToAction("Index", new { organizationId = organizationId.Value }); // Redirect to Index
                }
            }
            return View();
        }
    }
}
