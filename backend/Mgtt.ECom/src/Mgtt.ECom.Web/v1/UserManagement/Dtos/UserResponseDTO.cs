// <copyright file="UserResponseDTO.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.UserManagement.DTOs
{
    public class UserResponseDTO
    {
        public Guid UserID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
