// <copyright file="UserRequestDTO.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.UserManagement.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class UserRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }
    }
}
