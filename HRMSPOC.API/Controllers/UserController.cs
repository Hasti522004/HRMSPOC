﻿using HRMSPOC.API.Models;
using HRMSPOC.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        // Get user by ID (string from IdentityUser)
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Create new user
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> CreateUser([FromBody] ApplicationUser user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        // Update user (with string ID)
        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] ApplicationUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("User ID is Required.");
            }

            // Update user details
            try
            {
                await _userService.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating user: {ex.Message}");
            }

            return NoContent();
        }


        // Delete user (with string ID)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        [HttpGet("createdby/{createdbyId}")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsersByCreatedById(Guid createdbyId)
        {
            var users = await _userService.GetUsersByCreatedByIdAsync(createdbyId);
            return Ok(users);
        }
    }
}
