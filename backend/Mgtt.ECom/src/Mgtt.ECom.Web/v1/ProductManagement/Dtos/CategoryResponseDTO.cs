// <copyright file="CategoryResponseDTO.cs" company="PlaceholderCompany">
// Copyright (c) MGTheTrain. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ProductManagement.DTOs
{
    public class CategoryResponseDTO
    {
        public Guid CategoryID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
