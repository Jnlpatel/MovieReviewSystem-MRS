using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewSystem.Data;
using MovieReviewSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly MovieReviewSystemContext _context;

        public ReviewsController(MovieReviewSystemContext context)
        {
            _context = context;
        }

        // ✅ Get all reviews
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Movie)
                .Include(r => r.User)
                .Select(r => new ReviewDto
                {
                    ReviewID = r.ReviewID,
                    MovieID = r.MovieID,
                    MovieTitle = r.Movie.Title,
                    UserName = r.User.Name,
                    Rating = r.Rating,
                    ReviewText = r.ReviewText,
                    ReviewDate = r.ReviewDate
                })
                .ToListAsync();

            return Ok(reviews);
        }

        // ✅ Get a specific review by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Movie)
                .Include(r => r.User)
                .Where(r => r.ReviewID == id)
                .Select(r => new ReviewDto
                {
                    ReviewID = r.ReviewID,
                    MovieID = r.MovieID,
                    MovieTitle = r.Movie.Title,
                    UserName = r.User.Name,
                    Rating = r.Rating,
                    ReviewText = r.ReviewText,
                    ReviewDate = r.ReviewDate
                })
                .FirstOrDefaultAsync();

            if (review == null)
                return NotFound("Review not found");

            return Ok(review);
        }

        // ✅ Get all reviews for a specific movie
        [HttpGet("Movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByMovie(int movieId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.MovieID == movieId)
                .Include(r => r.Movie)
                .Include(r => r.User)
                .Select(r => new ReviewDto
                {
                    ReviewID = r.ReviewID,
                    MovieID = r.MovieID,
                    MovieTitle = r.Movie.Title,
                    UserName = r.User.Name,
                    Rating = r.Rating,
                    ReviewText = r.ReviewText,
                    ReviewDate = r.ReviewDate
                })
                .ToListAsync();

            return Ok(reviews);
        }

        // ✅ Get all reviews by a specific user
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsByUser(int userId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.UserID == userId)
                .Include(r => r.Movie)
                .Include(r => r.User)
                .Select(r => new ReviewDto
                {
                    ReviewID = r.ReviewID,
                    MovieID = r.MovieID,
                    MovieTitle = r.Movie.Title,
                    UserName = r.User.Name,
                    Rating = r.Rating,
                    ReviewText = r.ReviewText,
                    ReviewDate = r.ReviewDate
                })
                .ToListAsync();

            return Ok(reviews);
        }

        // ✅ Add a new review
        [HttpPost("Create")]
        public async Task<ActionResult<ReviewDto>> CreateReview(ReviewDto reviewDto)
        {
            var movie = await _context.Movies.FindAsync(reviewDto.MovieID);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == reviewDto.UserName);

            if (movie == null)
                return NotFound("Movie not found");

            if (user == null)
                return NotFound("User not found");

            var review = new Review
            {
                MovieID = reviewDto.MovieID,
                UserID = user.UserID,
                Rating = reviewDto.Rating,
                ReviewText = reviewDto.ReviewText,
                ReviewDate = reviewDto.ReviewDate
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = review.ReviewID }, reviewDto);
        }

        // ✅ Update a review
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateReview(int id, ReviewDto reviewDto)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return NotFound("Review not found");

            review.Rating = reviewDto.Rating;
            review.ReviewText = reviewDto.ReviewText;
            review.ReviewDate = reviewDto.ReviewDate;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Delete a review
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return NotFound("Review not found");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
