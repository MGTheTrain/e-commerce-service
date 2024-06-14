using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Domain.ReviewManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mgtt.ECom.Web.v1.ReviewManagement.DTOs;

namespace Mgtt.ECom.Web.v1.ReviewManagement.Controllers
{
    [Route("api/v1/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var review = new Review
            {
                ProductID = reviewDTO.ProductID,
                UserID = reviewDTO.UserID,
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,
                ReviewDate = DateTime.UtcNow
            };

            await _reviewService.CreateReview(review);

            var reviewResponseDTO = new ReviewResponseDTO
            {
                ReviewID = review.ReviewID,
                ProductID = review.ProductID,
                UserID = review.UserID,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };

            return CreatedAtAction(nameof(GetReviewById), reviewResponseDTO);
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
            var review = await _reviewService.GetReviewById(reviewId);

            if (review == null)
            {
                return NotFound(null);
            }

            var reviewDTO = new ReviewResponseDTO
            {
                ReviewID = review.ReviewID,
                ProductID = review.ProductID,
                UserID = review.UserID,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };

            return Ok(reviewDTO);
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
            var reviews = await _reviewService.GetReviewsByProductId(productId);
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
                    ReviewDate = review.ReviewDate
                });
            }

            return Ok(reviewDTOs);
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
            var reviews = await _reviewService.GetReviewsByUserId(userId);
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
                    ReviewDate = review.ReviewDate
                });
            }

            return Ok(reviewDTOs);
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="reviewId">The ID of the review to update.</param>
        /// <param name="reviewDTO">The review data transfer object containing updated review details.</param>
        /// <response code="204">If the review was successfully updated.</response>
        /// <response code="400">If the review data is invalid.</response>
        /// <response code="404">If the review is not found.</response>
        [HttpPut("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateReview(Guid reviewId, ReviewRequestDTO reviewDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var review = await _reviewService.GetReviewById(reviewId);

            if (review == null)
            {
                return NotFound(null);
            }

            review.ProductID = reviewDTO.ProductID;
            review.UserID = reviewDTO.UserID;
            review.Rating = reviewDTO.Rating;
            review.Comment = reviewDTO.Comment;

            await _reviewService.UpdateReview(review);

            var reviewResponseDTO = new ReviewResponseDTO
            {
                ReviewID = review.ReviewID,
                ProductID = review.ProductID,
                UserID = review.UserID,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };

            return Ok(reviewResponseDTO);
        }

        /// <summary>
        /// Deletes a review by its ID.
        /// </summary>
        /// <param name="reviewId">The ID of the review to delete.</param>
        /// <response code="204">If the review was successfully deleted.</response>
        /// <response code="404">If the review is not found.</response>
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(Guid reviewId)
        {
            var review = await _reviewService.GetReviewById(reviewId);

            if (review == null)
            {
                return NotFound(null);
            }

            await _reviewService.DeleteReview(reviewId);

            return NoContent();
        }
    }
}