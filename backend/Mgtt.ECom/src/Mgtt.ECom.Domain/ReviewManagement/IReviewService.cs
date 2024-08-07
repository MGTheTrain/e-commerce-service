// <copyright file="IReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Domain.ReviewManagement;

public interface IReviewService
{
    Task<IEnumerable<Review>?> GetAllReviews();

    Task<Review?> GetReviewById(Guid reviewId);

    Task<IEnumerable<Review>?> GetReviewsByProductId(Guid productId);

    Task<IEnumerable<Review>?> GetReviewsByUserId(string userId);

    Task<Review?> CreateReview(Review review);

    Task<Review?> UpdateReview(Review review);

    Task DeleteReview(Guid reviewId);
}
