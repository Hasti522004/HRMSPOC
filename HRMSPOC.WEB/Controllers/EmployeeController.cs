using HRMSPOC.WEB.DTOs;
using HRMSPOC.WEB.Models;
using HRMSPOC.WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.WEB.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: HR/SetHRId/{HrId}
        public IActionResult SetHRId(Guid hrId)
        {
            HttpContext.Session.SetString("HrId", hrId.ToString());
            return RedirectToAction("Index", "Employee"); // Redirect to Employee Index
        }

        // Private method to retrieve HrId from session
        private Guid? GetHrIdFromSession()
        {
            var hrIdString = HttpContext.Session.GetString("HrId");
            if (Guid.TryParse(hrIdString, out var hrId))
            {
                return hrId;
            }
            return null; // Return null if the ID is not found or not valid
        }

        // GET: Employee/Index
        public async Task<IActionResult> Index()
        {
            var hrId = GetHrIdFromSession();

            if (hrId.HasValue)
            {
                var users = await _employeeService.GetUsersByHrIdAsync(hrId.Value);
                ViewBag.HrId = hrId.Value; // Pass the HR ID to the view
                return View(users);
            }
            return RedirectToAction("Index", "Employee"); // Redirect if no HR ID is found
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            var hrId = GetHrIdFromSession();
            if (hrId.HasValue)
            {
                ViewBag.HrId = hrId.Value;
                return View();
            }
            return RedirectToAction("Index", "Employee"); // Redirect if no HR ID is found
        }

        // POST: Employee/Create
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUserViewModel user)
        {
            var hrId = GetHrIdFromSession();
            if (hrId.HasValue)
            {
                user.CreatedBy = hrId.Value; // Set CreatedBy field

                if (ModelState.IsValid)
                {
                    if (await _employeeService.CreateUserAsync(user))
                    {
                        return RedirectToAction("Index", "Employee"); // Redirect to Index
                    }
                }
            }
            return View(user);
        }

        // GET: Employee/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var userDto = await _employeeService.GetUserByIdAsync(id);
            if (userDto != null)
            {
                ViewBag.UserId = id; // Store the UserId to use in POST action
                return View(userDto); // Return the DTO to the view
            }
            return NotFound();
        }

        // POST: Employee/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(userDto.Id))
                {
                    return BadRequest("User ID cannot be null or empty.");
                }

                if (await _employeeService.UpdateUserAsync(userDto))
                {
                    return RedirectToAction("Index", "Employee"); // Redirect to Index
                }
            }
            return View(userDto); // Return the DTO back to the view if validation fails
        }

        // GET: Employee/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _employeeService.GetUserToDeleteAsync(id);
            if (user != null)
            {
                return View(user);
            }
            return NotFound();
        }

        // POST: Employee/DeleteConfirmed/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await _employeeService.DeleteUserAsync(id))
            {
                var hrId = GetHrIdFromSession(); // Use the session to get the HrId
                if (hrId.HasValue)
                {
                    return RedirectToAction("Index", new { hrId = hrId.Value }); // Redirect to Index
                }
            }
            return View();
        }
    }
}
