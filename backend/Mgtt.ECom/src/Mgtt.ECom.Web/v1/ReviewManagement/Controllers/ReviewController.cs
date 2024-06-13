using Microsoft.AspNetCore.Mvc;
using Mgtt.ECom.Application.DTOs.Review;
using Mgtt.ECom.Domain.ReviewManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewRequestDTO reviewDTO)
        {
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

            return CreatedAtAction(nameof(GetReviewById), new { reviewId = reviewResponseDTO.ReviewID }, reviewResponseDTO);
        }

        [HttpGet("{reviewId}")]
        public async Task<ActionResult<ReviewResponseDTO>> GetReviewById(Guid reviewId)
        {
            var review = await _reviewService.GetReviewById(reviewId);

            if (review == null)
            {
                return NotFound();
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

        [HttpGet("product/{productId}")]
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

        [HttpGet("user/{userId}")]
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

        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(Guid reviewId, ReviewRequestDTO reviewDTO)
        {
            var existingReview = await _reviewService.GetReviewById(reviewId);

            if (existingReview == null)
            {
                return NotFound();
            }

            existingReview.ProductID = reviewDTO.ProductID;
            existingReview.UserID = reviewDTO.UserID;
            existingReview.Rating = reviewDTO.Rating;
            existingReview.Comment = reviewDTO.Comment;

            await _reviewService.UpdateReview(existingReview);

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(Guid reviewId)
        {
            var existingReview = await _reviewService.GetReviewById(reviewId);

            if (existingReview == null)
            {
                return NotFound();
            }

            await _reviewService.DeleteReview(reviewId);

            return NoContent();
        }
    }
}
