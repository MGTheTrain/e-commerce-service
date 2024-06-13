using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Web.v1.ProductManagement.DTOs
{
    public class CategoryRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
