// <copyright file="ReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.ReviewManagement;
    using Mgtt.ECom.Persistence.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ReviewService : IReviewService
    {
        private readonly PsqlDbContext context;
        private readonly ILogger<ReviewService> logger;

        public ReviewService(PsqlDbContext context, ILogger<ReviewService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<Review?> GetReviewById(Guid reviewId)
        {
            this.logger.LogInformation("Fetching review by ID: {ReviewId}", reviewId);
            try
            {
                return await this.context.Reviews.FindAsync(reviewId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching review by ID: {ReviewId}", reviewId);
                return await Task.FromResult<Review?>(null);
            }
        }

        public async Task<IEnumerable<Review>?> GetAllReviews()
        {
            this.logger.LogInformation("Fetching all reviews");
            try
            {
                return await this.context.Reviews.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching all reviews");
                return await Task.FromResult<IEnumerable<Review>?>(null);
            }
        }

        public async Task<IEnumerable<Review>?> GetReviewsByProductId(Guid productId)
        {
            this.logger.LogInformation("Fetching reviews by Product ID: {ProductId}", productId);
            try
            {
                return await this.context.Reviews.Where(r => r.ProductID == productId).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching reviews by Product ID: {ProductId}", productId);
                return await Task.FromResult<IEnumerable<Review>?>(null);
            }
        }

        public async Task<IEnumerable<Review>?> GetReviewsByUserId(string userId)
        {
            this.logger.LogInformation("Fetching reviews by User ID: {UserId}", userId);
            try
            {
                return await this.context.Reviews.Where(r => r.UserID == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error fetching reviews by User ID: {UserId}", userId);
                return await Task.FromResult<IEnumerable<Review>?>(null);
            }
        }

        public async Task<Review?> CreateReview(Review review)
        {
            this.logger.LogInformation("Creating new review for Product ID: {ProductId}, User ID: {UserId}", review.ProductID, review.UserID);
            try
            {
                this.context.Reviews.Add(review);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Review created successfully: {ReviewId}", review.ReviewID);
                return await Task.FromResult<Review?>(review);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error creating review for Product ID: {ProductId}, User ID: {UserId}", review.ProductID, review.UserID);
                return await Task.FromResult<Review?>(null);
            }
        }

        public async Task<Review?> UpdateReview(Review review)
        {
            this.logger.LogInformation("Updating review: {ReviewId}", review.ReviewID);
            try
            {
                this.context.Reviews.Update(review);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Review updated successfully: {ReviewId}", review.ReviewID);
                return await Task.FromResult<Review?>(review);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error updating review: {ReviewId}", review.ReviewID);
                return await Task.FromResult<Review?>(null);
            }
        }

        public async Task DeleteReview(Guid reviewId)
        {
            this.logger.LogInformation("Deleting review: {ReviewId}", reviewId);
            try
            {
                var review = await this.context.Reviews.FindAsync(reviewId);
                if (review != null)
                {
                    this.context.Reviews.Remove(review);
                    await this.context.SaveChangesAsync();
                    this.logger.LogInformation("Review deleted successfully: {ReviewId}", reviewId);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error deleting review: {ReviewId}", reviewId);
            }
        }
    }
}