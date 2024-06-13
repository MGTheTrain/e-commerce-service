using Mgtt.ECom.Web.v1.UserManagement.DTOs;
using Mgtt.ECom.Domain.UserManagement;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user with the specified ID.</returns>
        /// <response code="200">Returns the user with the specified ID.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userRequest">The user data transfer object containing user details.</param>
        /// <returns>The newly created user.</returns>
        /// <response code="201">Returns the newly created user.</response>
        /// <response code="400">If the user data is invalid.</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userRequest">The user data transfer object containing updated user details.</param>
        /// <response code="204">If the user was successfully updated.</response>
        /// <response code="400">If the user data is invalid.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <response code="204">If the user was successfully deleted.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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