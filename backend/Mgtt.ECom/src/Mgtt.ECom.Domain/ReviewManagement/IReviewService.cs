namespace Mgtt.ECom.Domain.ReviewManagement;

public interface IReviewService
{
    Review GetReviewById(int reviewId);
    IEnumerable<Review> GetReviewsByProductId(int productId);
    IEnumerable<Review> GetReviewsByUserId(int userId);
    void CreateReview(Review review);
    void UpdateReview(Review review);
    void DeleteReview(int reviewId);
}
