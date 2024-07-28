// <copyright file="ProductResponseDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ProductManagement.DTOs
{
    public class ProductResponseDTO
    {
        public Guid ProductID { get; set; }

        public string UserID { get; set; }

        public List<string>? Categories { get; set; }

        public string Name { get; set; }

        public string SnapShotImageName { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public int Stock { get; set; }

        public string ImageUrl { get; set; }
    }
}
