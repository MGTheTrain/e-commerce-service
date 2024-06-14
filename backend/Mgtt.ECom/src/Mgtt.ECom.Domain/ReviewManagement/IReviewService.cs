namespace Mgtt.ECom.Domain.ReviewManagement;

public interface IReviewService
{
    Task<Review?> GetReviewById(Guid reviewId);
    Task<IEnumerable<Review>?> GetReviewsByProductId(Guid productId);
    Task<IEnumerable<Review>?> GetReviewsByUserId(Guid userId);
    Task<Review?> CreateReview(Review review);
    Task<Review?> UpdateReview(Review review);
    Task DeleteReview(Guid reviewId);
}
