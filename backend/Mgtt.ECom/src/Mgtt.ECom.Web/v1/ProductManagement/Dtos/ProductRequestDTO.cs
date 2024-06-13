namespace Mgtt.ECom.Web.v1.ProductManagement.DTOs
{
    public class ProductRequestDTO
    {
        public Guid CategoryID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public int Stock { get; set; }

        public string ImageUrl { get; set; }
    }
}
