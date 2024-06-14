using Mgtt.ECom.Domain.ReviewManagement;
using Mgtt.ECom.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mgtt.ECom.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly PsqlDbContext _context;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(PsqlDbContext context, ILogger<ReviewService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Review?> GetReviewById(Guid reviewId)
        {
            _logger.LogInformation("Fetching review by ID: {ReviewId}", reviewId);
            try
            {
                return await _context.Reviews.FindAsync(reviewId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching review by ID: {ReviewId}", reviewId);
                return await Task.FromResult<Review?>(null);
            }
        }

        public async Task<IEnumerable<Review>?> GetReviewsByProductId(Guid productId)
        {
            _logger.LogInformation("Fetching reviews by Product ID: {ProductId}", productId);
            try
            {
                return await _context.Reviews.Where(r => r.ProductID == productId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reviews by Product ID: {ProductId}", productId);
                return await Task.FromResult<IEnumerable<Review>?>(null);
            }
        }

        public async Task<IEnumerable<Review>?> GetReviewsByUserId(Guid userId)
        {
            _logger.LogInformation("Fetching reviews by User ID: {UserId}", userId);
            try
            {
                return await _context.Reviews.Where(r => r.UserID == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reviews by User ID: {UserId}", userId);
                return await Task.FromResult<IEnumerable<Review>?>(null);
            }
        }

        public async Task<Review?> CreateReview(Review review)
        {
            _logger.LogInformation("Creating new review for Product ID: {ProductId}, User ID: {UserId}", review.ProductID, review.UserID);
            try
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Review created successfully: {ReviewId}", review.ReviewID);
                return await Task.FromResult<Review?>(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review for Product ID: {ProductId}, User ID: {UserId}", review.ProductID, review.UserID);
                return await Task.FromResult<Review?>(null);
            }
        }

        public async Task<Review?> UpdateReview(Review review)
        {
            _logger.LogInformation("Updating review: {ReviewId}", review.ReviewID);
            try
            {
                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Review updated successfully: {ReviewId}", review.ReviewID);
                return await Task.FromResult<Review?>(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review: {ReviewId}", review.ReviewID);
                return await Task.FromResult<Review?>(null);
            }
        }

        public async Task DeleteReview(Guid reviewId)
        {
            _logger.LogInformation("Deleting review: {ReviewId}", reviewId);
            try
            {
                var review = await _context.Reviews.FindAsync(reviewId);
                if (review != null)
                {
                    _context.Reviews.Remove(review);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Review deleted successfully: {ReviewId}", reviewId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review: {ReviewId}", reviewId);
            }
        }
    }
}