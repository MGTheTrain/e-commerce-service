// <copyright file="UserController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.UserManagement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.UserManagement;
    using Mgtt.ECom.Web.V1.UserManagement.DTOs;
    using Mgtt.ECom.Web.V1.UserManagement.DTOs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/v1/users")]
    [Authorize("manage:users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
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
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = new User
            {
                UserName = userRequest.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password),
                Email = userRequest.Email,
                Role = userRequest.Role,
            };

            var action = await this.userService.CreateUser(user);
            if (action == null)
            {
                return this.BadRequest(); // preferable return a proper

                // response DTO with proper error message
            }

            var userResponseDTO = new UserResponseDTO
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
            };

            return this.CreatedAtAction(nameof(this.Register), userResponseDTO);
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
            var user = await this.userService.GetUserById(id);
            if (user == null)
            {
                return this.NotFound();
            }

            var userResponse = new UserResponseDTO
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
            };
            return this.Ok(userResponse);
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        /// <response code="200">Returns a list of all users.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponseDTO>))]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers()
        {
            var users = await this.userService.GetAllUsers();
            var userDTOs = new List<UserResponseDTO>();

            foreach (var user in users)
            {
                userDTOs.Add(new UserResponseDTO
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                });
            }

            return this.Ok(userDTOs);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userRequest">The user data transfer object containing updated user details.</param>
        /// <response code="204">If the user was successfully updated.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="400">If the user data is invalid.</response>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(Guid id, UserRequestDTO userRequest)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = await this.userService.GetUserById(id);
            if (user == null)
            {
                return this.NotFound();
            }

            user.UserName = userRequest.UserName;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            user.Email = userRequest.Email;
            user.Role = userRequest.Role;

            var action = await this.userService.UpdateUser(user);
            if (action == null)
            {
                return this.BadRequest();
            }

            var userResponseDTO = new UserResponseDTO
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
            };

            return this.Ok(userResponseDTO);
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <response code="204">If the user was successfully deleted.</response>
        /// <response code="404">If the user is not found.</response>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await this.userService.GetUserById(id);
            if (user == null)
            {
                return this.NotFound();
            }

            await this.userService.DeleteUser(id);

            return this.NoContent();
        }
    }
}