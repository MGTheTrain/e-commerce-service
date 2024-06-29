// <copyright file="CategoryRequestDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ProductManagement.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
