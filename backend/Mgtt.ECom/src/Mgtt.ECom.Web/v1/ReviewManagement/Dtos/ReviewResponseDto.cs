namespace Mgtt.ECom.Web.v1.ReviewManagement.DTOs
{
    public class ReviewResponseDTO
    {
        public Guid ReviewID { get; set; }
        public Guid ProductID { get; set; }
        public Guid UserID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}