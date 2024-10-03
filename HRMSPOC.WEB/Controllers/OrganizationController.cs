using HRMSPOC.WEB.Models;
using HRMSPOC.WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.WEB.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly OrganizationService _organizationService;

        public OrganizationController(OrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        // GET: Organization
        public async Task<IActionResult> Index()
        {
            var organizations = await _organizationService.GetOrganizationsAsync();
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
                var success = await _organizationService.CreateOrganizationAsync(organization);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(organization);
        }

        // GET: Organization/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var organization = await _organizationService.GetOrganizationByIdAsync(id);
            return View(organization);
        }

        // POST: Organization/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, OrganizationViewModel organization)
        {
            if (ModelState.IsValid)
            {
                var success = await _organizationService.UpdateOrganizationAsync(id, organization);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(organization);
        }

        // GET: Organization/Delete/{id}
        public async Task<IActionResult> Delete(Guid id)
        {
            var organization = await _organizationService.GetOrganizationByIdAsync(id);
            return View(organization);
        }

        // POST: Organization/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _organizationService.DeleteOrganizationAsync(id);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult ManageHR(Guid organizationId)
        {
            HttpContext.Session.SetString("OrganizationId", organizationId.ToString());
            return RedirectToAction("Index", "User"); 
        }
    }
}
