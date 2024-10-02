using HRMSPOC.API.DTOs;
using HRMSPOC.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
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
        //[Authorize]
        public async Task<IActionResult> GetOrganizationIdByUserId(string userId)
        {
            var organizationId = await _userOrganizationService.GetOrganizationIdByUserIdAsync(userId);

            if (!organizationId.HasValue)
            {
                return NotFound();
            }

            return Ok(new { OrganizationId = organizationId.Value });
        }
        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> AddUserToOrganization([FromBody] UserOrganizationDto userOrganizationDto)
        {
            if (string.IsNullOrEmpty(userOrganizationDto.UserId) || userOrganizationDto.OrganizationId == Guid.Empty)
            {
                return BadRequest("User ID and Organization ID cannot be empty.");
            }

            bool isCreated = await _userOrganizationService.AddUserOrganizationAsync(userOrganizationDto);
            return Ok("User successfully added to organization.");
        }
    }
}
