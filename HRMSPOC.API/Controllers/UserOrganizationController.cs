using HRMSPOC.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserOrganizationController : ControllerBase
    {
        private readonly IUserOrganizationService _userOrganizationService;

        public UserOrganizationController(IUserOrganizationService userOrganizationService)
        {
            _userOrganizationService = userOrganizationService;
        }

        // GET: api/UserOrganization/organization/{userId}
        [HttpGet("organization/{userId}")]
        public async Task<IActionResult> GetOrganizationIdByUserId(string userId)
        {
            var organizationId = await _userOrganizationService.GetOrganizationIdByUserIdAsync(userId);

            if (!organizationId.HasValue)
            {
                return NotFound(); // Return 404 if not found
            }

            return Ok(new { OrganizationId = organizationId.Value });
        }
    }
}
