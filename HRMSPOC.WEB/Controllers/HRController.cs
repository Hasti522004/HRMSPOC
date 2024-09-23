using HRMSPOC.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HRMSPOC.WEB.Controllers
{
    public class HRController : Controller
    {
        private readonly HttpClient _httpClient;

        public HRController()
        {
            _httpClient = new HttpClient();
        }

        // GET: HR/ManageHR/5
        public async Task<IActionResult> Index(int organizationId)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7095/api/Employee?organizationId={organizationId}");
            var employees = JsonConvert.DeserializeObject<List<Employee>>(response);

            ViewBag.OrganizationId = organizationId;
            return View(employees);
        }

        // GET: HR/Create
        public IActionResult Create(int organizationId)
        {
            ViewBag.OrganizationId = organizationId;
            return View();
        }

        // POST: HR/Create
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7095/api/Employee", employee);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { organizationId = employee.OrganizationId });
                }
            }
            return View(employee);
        }

        // GET: HR/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7095/api/Employee/{id}");
            var employee = JsonConvert.DeserializeObject<Employee>(response);
            return View(employee);
        }

        // POST: HR/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/Employee/{employee.Id}", employee);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { organizationId = employee.OrganizationId });
                }
            }
            return View(employee);
        }

        // GET: HR/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetStringAsync($"https://localhost:7095/api/Employee/{id}");
            var employee = JsonConvert.DeserializeObject<Employee>(response);
            return View(employee);
        }

        // POST: HR/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7095/api/Employee/{id}");
            if (response.IsSuccessStatusCode)
            {
                // Assuming you want to redirect to the organization HR management page
                // You may need to pass the organizationId; adjust as necessary
                return RedirectToAction("Index", new { organizationId = id }); // Change this as necessary
            }
            return View();
        }
    }
}
