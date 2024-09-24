using HRMSPOC.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HRMSPOC.WEB.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrganizationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: Organization
        public async Task<IActionResult> Index()
        {
            // Fetch organizations
            var response = await _httpClient.GetStringAsync("https://localhost:7095/api/Organization");
            var organizations = JsonConvert.DeserializeObject<List<OrganizationViewModel>>(response);
            return View(organizations);
        }

        // GET: Organization/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organization/Create
        [HttpPost]
        public async Task<IActionResult> Create(OrganizationViewModel organization)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7095/api/Organization", organization);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(organization);
        }

        // GET: Organization/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7095/api/Organization/{id}");
            var organization = JsonConvert.DeserializeObject<OrganizationViewModel>(response);
            return View(organization);
        }

        // POST: Organization/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, OrganizationViewModel organization)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/Organization/{id}", organization);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(organization);
        }

        // GET: Organization/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7095/api/Organization/{id}");
            var organization = JsonConvert.DeserializeObject<OrganizationViewModel>(response);
            return View(organization);
        }

        // POST: Organization/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7095/api/Organization/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // Redirect to manage HR while setting the organization ID
        public IActionResult ManageHR(Guid organizationId)
        {
            // Set the OrganizationId in the session
            HttpContext.Session.SetString("OrganizationId", organizationId.ToString());
            return RedirectToAction("Index", "User"); // Redirect to User Index to manage users for the organization
        }
    }
}
