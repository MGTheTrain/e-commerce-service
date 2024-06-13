namespace Mgtt.ECom.Web.v1.ProductManagement.DTOs
{
    public class ProductRequestDTO
    {
        [Required]
        public Guid CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public float Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative")]
        public int Stock { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}