namespace Mgtt.ECom.Domain.ReviewManagement;

public interface IReviewService
{
    Task<Review> GetReviewById(int reviewId);
    Task<IEnumerable<Review>> GetReviewsByProductId(int productId);
    Task<IEnumerable<Review>> GetReviewsByUserId(int userId);
    Task CreateReview(Review review);
    Task UpdateReview(Review review);
    Task DeleteReview(int reviewId);
}
