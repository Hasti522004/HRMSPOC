using HRMSPOC.API.Models;
using HRMSPOC.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.API.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]

    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        // Get all organizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> GetAllOrganizations()
        {
            var organizations = await _organizationService.GetOrganizationsAsync();
            return Ok(organizations);
        }

        // Get organization by ID (Guid)
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Organization>> GetOrganizationById(Guid id)
        {
            var organization = await _organizationService.GetOrganizationByIdAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return Ok(organization);
        }

        // Create new organization
        [HttpPost]
        public async Task<ActionResult<Organization>> CreateOrganization([FromBody] Organization organization)
        {
            if (organization == null)
            {
                return BadRequest("Organization data is required.");
            }

            // Replace with actual SuperAdmin ID logic
            Guid superAdminId = new Guid("70cd23a7-e069-4f21-93ca-d862c72964e4");
            var createdOrganization = await _organizationService.CreateOrganizationWithAdminAsync(organization, superAdminId);
            return CreatedAtAction(nameof(GetOrganizationById), new { id = createdOrganization.Id }, createdOrganization);
        }

        // Update organization (with Guid ID)
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateOrganization(Guid id, [FromBody] Organization organization)
        {
            if (id != organization.Id)
            {
                return BadRequest("Organization ID mismatch.");
            }
            await _organizationService.UpdateOrganizationAsync(organization);
            return NoContent();
        }

        // Delete organization (with Guid ID)
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteOrganization(Guid id)
        {
            await _organizationService.DeleteOrganizationAsync(id);
            return NoContent();
        }

        [HttpGet("IsOrganizationExist/{id:guid}")]
        public async Task<bool> IsOrganizationExists(Guid id)
        {
            bool isorgExist = await _organizationService.IsOrganizationExists(id);
            return isorgExist;
        }
    }
}
