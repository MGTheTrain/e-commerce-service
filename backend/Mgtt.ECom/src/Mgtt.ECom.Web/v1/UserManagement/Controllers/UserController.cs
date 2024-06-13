using Mgtt.ECom.Web.v1.UserManagement.DTOs;
using Mgtt.ECom.Domain.UserManagement;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Mgtt.ECom.Web.v1.UserManagement.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userResponse = new UserResponseDTO
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
            return Ok(userResponse);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDTO>> Register(UserRequestDTO userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = userRequest.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password),
                Email = userRequest.Email,
                Role = userRequest.Role
            };

            await _userService.CreateUser(user);

            var userResponse = new UserResponseDTO
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };

            return CreatedAtAction(nameof(GetUserById), new { id = userResponse.UserID }, userResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserRequestDTO userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.UserName = userRequest.UserName;
            existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            existingUser.Email = userRequest.Email;
            existingUser.Role = userRequest.Role;

            await _userService.UpdateUser(existingUser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var existingUser = await _userService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userService.DeleteUser(id);

            return NoContent();
        }
    }
}
