using Mgtt.ECom.Domain.ReviewManagement;
using Mgtt.ECom.Persistence.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly PsqlDbContext _context;

        public ReviewService(PsqlDbContext context)
        {
            _context = context;
        }

        public async Task<Review> GetReviewById(int reviewId)
        {
            return await Task.FromResult(_context.Reviews.Find(reviewId));
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductId(int productId)
        {
            return await Task.FromResult(_context.Reviews.Where(r => r.ProductID == productId).ToList());
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserId(int userId)
        {
            return await Task.FromResult(_context.Reviews.Where(r => r.UserID == userId).ToList());
        }

        public async Task CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReview(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}
