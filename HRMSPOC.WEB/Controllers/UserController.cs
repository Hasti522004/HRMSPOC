﻿using HRMSPOC.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HRMSPOC.WEB.DTOs;

namespace HRMSPOC.WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Organization/SetOrganizationId/{organizationId}
        public IActionResult SetOrganizationId(Guid organizationId)
        {
            HttpContext.Session.SetString("OrganizationId", organizationId.ToString());
            return RedirectToAction("Index", "User"); // Redirect to User Index
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
                var response = await _httpClient.GetStringAsync($"https://localhost:7095/api/User/createdby/{organizationId.Value}");
                var users = JsonConvert.DeserializeObject<List<ApplicationUserViewModel>>(response);
                ViewBag.OrganizationId = organizationId.Value; // Pass the organization ID to the view
                return View(users);
            }
            return RedirectToAction("Index", "User"); // Redirect if no organization ID is found
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
            return RedirectToAction("Index", "User"); // Redirect if no organization ID is found
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
                    // Send the object without Id to the API
                    var response = await _httpClient.PostAsJsonAsync("https://localhost:7095/api/User", user);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index"); // Redirect to Index
                    }
                }
            }
            return View(user);
        }

        // GET: User/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7095/api/User/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContent}");

                // Deserialize to ApplicationUserDto
                var userDto = JsonConvert.DeserializeObject<ApplicationUserDto>(responseContent);

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

                // Send the DTO in the PUT request
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/User", userDto);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Redirect to Index
                }
            }
            return View(userDto); // Return the DTO back to the view if validation fails
        }

        // GET: User/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7095/api/User/{id}");
            if (response.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<ApplicationUserViewModel>(await response.Content.ReadAsStringAsync());
                return View(user);
            }
            return NotFound();
        }

        // POST: User/DeleteConfirmed/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7095/api/User/{id}");
            if (response.IsSuccessStatusCode)
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
