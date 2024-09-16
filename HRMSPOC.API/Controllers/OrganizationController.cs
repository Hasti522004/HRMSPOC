using HRMSPOC.API.Models;
using HRMSPOC.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly OrganizationService _organizationService;
        public OrganizationController(OrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> GetAllOrganization()
        {
            var orgs = await _organizationService.GetOrganizationsAsync();
            return Ok(orgs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganizationById(int id)
        {
            var org = await _organizationService.GetOrganizationByIdAsync(id);
            return Ok(org);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrganization([FromBody] Organization organization)
        {
            await _organizationService.CreateOrganizationAsync(organization);
            return CreatedAtAction(nameof(GetOrganizationById),new {id = organization.Id},organization);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrganization(int id, [FromBody] Organization organization)
        {
            if(id != organization.Id)
            {
                return BadRequest();
            }
            await _organizationService.UpdateOrganizationAsync(organization);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrganization(int id)
        {
            await _organizationService.DeleteOrganizationAsync(id);
            return NoContent();
        }
    }
}
