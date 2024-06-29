// <copyright file="ReviewController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.V1.ReviewManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Mgtt.ECom.Domain.ReviewManagement;
    using Mgtt.ECom.Web.V1.ReviewManagement.DTOs;
    using Mgtt.ECom.Web.V1.UserManagement.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using static System.Collections.Specialized.BitVector32;

    [Route("api/v1/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="reviewDTO">The review data transfer object containing review details.</param>
        /// <returns>The newly created review.</returns>
        /// <response code="201">Returns the newly created review.</response>
        /// <response code="400">If the review data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReviewResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateReview(ReviewRequestDTO reviewDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var review = new Review
            {
                ProductID = reviewDTO.ProductID,
                UserID = reviewDTO.UserID,
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,
                ReviewDate = DateTime.UtcNow,
            };

            var action = await this.reviewService.CreateReview(review);
            if (action == null)
            {
                return this.BadRequest();
            }

            var reviewResponseDTO = new ReviewResponseDTO
            {
                ReviewID = review.ReviewID,
                ProductID = review.ProductID,
                UserID = review.UserID,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate,
            };

            return this.CreatedAtAction(nameof(this.CreateReview), reviewResponseDTO);
        }

        /// <summary>
        /// Retrieves a review by its ID.
        /// </summary>
        /// <param name="reviewId">The ID of the review.</param>
        /// <returns>The review with the specified ID.</returns>
        /// <response code="200">Returns the review with the specified ID.</response>
        /// <response code="404">If the review is not found.</response>
        [HttpGet("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponseDTO>> GetReviewById(Guid reviewId)
        {
            var review = await this.reviewService.GetReviewById(reviewId);

            if (review == null)
            {
                return this.NotFound();
            }

            var reviewDTO = new ReviewResponseDTO
            {
                ReviewID = review.ReviewID,
                ProductID = review.ProductID,
                UserID = review.UserID,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate,
            };

            return this.Ok(reviewDTO);
        }

        /// <summary>
        /// Retrieves all reviews.
        /// </summary>
        /// <returns>A list of all reviews.</returns>
        /// <response code="200">Returns a list of all reviews.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReviewResponseDTO>))]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetAllReviews()
        {
            var reviews = await this.reviewService.GetAllReviews();
            var reviewDTOs = new List<ReviewResponseDTO>();

            foreach (var review in reviews)
            {
                reviewDTOs.Add(new ReviewResponseDTO
                {
                    ProductID = review.ProductID,
                    UserID = review.UserID,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    ReviewDate = review.ReviewDate,
                });
            }

            return this.Ok(reviewDTOs);
        }

        /// <summary>
        /// Retrieves reviews by product ID.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>A list of reviews for the specified product.</returns>
        /// <response code="200">Returns a list of reviews for the specified product.</response>
        [HttpGet("product/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReviewResponseDTO>))]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetReviewsByProductId(Guid productId)
        {
            var reviews = await this.reviewService.GetReviewsByProductId(productId);
            var reviewDTOs = new List<ReviewResponseDTO>();

            foreach (var review in reviews)
            {
                reviewDTOs.Add(new ReviewResponseDTO
                {
                    ReviewID = review.ReviewID,
                    ProductID = review.ProductID,
                    UserID = review.UserID,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    ReviewDate = review.ReviewDate,
                });
            }

            return this.Ok(reviewDTOs);
        }

        /// <summary>
        /// Retrieves reviews by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of reviews by the specified user.</returns>
        /// <response code="200">Returns a list of reviews by the specified user.</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReviewResponseDTO>))]
        public async Task<ActionResult<IEnumerable<ReviewResponseDTO>>> GetReviewsByUserId(Guid userId)
        {
            var reviews = await this.reviewService.GetReviewsByUserId(userId);
            var reviewDTOs = new List<ReviewResponseDTO>();

            foreach (var review in reviews)
            {
                reviewDTOs.Add(new ReviewResponseDTO
                {
                    ReviewID = review.ReviewID,
                    ProductID = review.ProductID,
                    UserID = review.UserID,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    ReviewDate = review.ReviewDate,
                });
            }

            return this.Ok(reviewDTOs);
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="reviewId">The ID of the review to update.</param>
        /// <param name="reviewDTO">The review data transfer object containing updated review details.</param>
        /// <response code="204">If the review was successfully updated.</response>
        /// <response code="400">If the review data is invalid.</response>
        /// <response code="404">If the review is not found.</response>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpPut("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateReview(Guid reviewId, ReviewRequestDTO reviewDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var review = await this.reviewService.GetReviewById(reviewId);

            if (review == null)
            {
                return this.NotFound();
            }

            review.ProductID = reviewDTO.ProductID;
            review.UserID = reviewDTO.UserID;
            review.Rating = reviewDTO.Rating;
            review.Comment = reviewDTO.Comment;

            var action = await this.reviewService.UpdateReview(review);
            if (action == null)
            {
                return this.BadRequest();
            }

            var reviewResponseDTO = new ReviewResponseDTO
            {
                ReviewID = review.ReviewID,
                ProductID = review.ProductID,
                UserID = review.UserID,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate,
            };

            return this.Ok(reviewResponseDTO);
        }

        /// <summary>
        /// Deletes a review by its ID.
        /// </summary>
        /// <param name="reviewId">The ID of the review to delete.</param>
        /// <response code="204">If the review was successfully deleted.</response>
        /// <response code="404">If the review is not found.</response>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(Guid reviewId)
        {
            var review = await this.reviewService.GetReviewById(reviewId);

            if (review == null)
            {
                return this.NotFound();
            }

            await this.reviewService.DeleteReview(reviewId);

            return this.NoContent();
        }
    }
}