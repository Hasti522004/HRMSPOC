using HRMSPOC.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HRMSPOC.WEB.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrganizationController()
        {
            _httpClient = new HttpClient();
        }

        // GET: Organization
        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("https://localhost:7095/api/Organization");
            var organizations = JsonConvert.DeserializeObject<List<Organization>>(response);
            return View(organizations);
        }

        // GET: Organization/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organization/Create
        [HttpPost]

        public async Task<IActionResult> Create(Organization organization)
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


        // GET: Organization/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7095/api/Organization/{id}");
            var organization = JsonConvert.DeserializeObject<Organization>(response);
            return View(organization);
        }

        // POST: Organization/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Organization organization)
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

        // GET: Organization/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7095/api/Organization/{id}");
            var organization = JsonConvert.DeserializeObject<Organization>(response);
            return View(organization);
        }

        // POST: Organization/Delete/5
        // POST: Organization/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7095/api/Organization/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

    }
}
