using Microsoft.AspNetCore.Mvc;
using MovieReviewSystem.Models;
using Microsoft.EntityFrameworkCore;
using MovieReviewSystem.Data;

namespace MovieReviewSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesApiController : ControllerBase
    {
        private readonly MovieReviewSystemContext _context;

        public MoviesApiController(MovieReviewSystemContext context)
        {
            _context = context;
        }

        // GET: api/MoviesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Select(m => new MovieDto
                {
                    MovieID = m.MovieID,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Director = m.Director,
                    Description = m.Description,
                    PosterURL = m.PosterURL,
                    AverageRating = m.Reviews.Any() ? m.Reviews.Average(r => r.Rating) : 0,
                    Genres = m.MovieGenres.Select(mg => new GenreDto { GenreID = mg.Genre.GenreID, Name = mg.Genre.Name }).ToList()
                })
                .ToListAsync();

            return Ok(movies);
        }

        // GET: api/MoviesApi/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Where(m => m.MovieID == id)
                .Select(m => new MovieDto
                {
                    MovieID = m.MovieID,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Director = m.Director,
                    Description = m.Description,
                    PosterURL = m.PosterURL,
                    AverageRating = m.Reviews.Any() ? m.Reviews.Average(r => r.Rating) : 0,
                    Genres = m.MovieGenres.Select(mg => new GenreDto { GenreID = mg.Genre.GenreID, Name = mg.Genre.Name }).ToList()
                })
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // POST: api/MoviesApi
        [HttpPost]
        public async Task<ActionResult<MovieDto>> CreateMovie([FromBody] CreateMovieRequestDto createMovieRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var movie = new Movie
                {
                    Title = createMovieRequest.Title,
                    ReleaseDate = createMovieRequest.ReleaseDate,
                    Director = createMovieRequest.Director,
                    Description = createMovieRequest.Description,
                    PosterURL = createMovieRequest.PosterURL
                };

                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();

                foreach (var genreId in createMovieRequest.GenreIds)
                {
                    _context.MovieGenres.Add(new MovieGenre { MovieID = movie.MovieID, GenreID = genreId });
                }

                await _context.SaveChangesAsync();

                var movieDto = new MovieDto
                {
                    MovieID = movie.MovieID,
                    Title = movie.Title,
                    ReleaseDate = movie.ReleaseDate,
                    Director = movie.Director,
                    Description = movie.Description,
                    PosterURL = movie.PosterURL,
                    Genres = await _context.MovieGenres
                        .Where(mg => mg.MovieID == movie.MovieID)
                        .Select(mg => new GenreDto { GenreID = mg.Genre.GenreID, Name = mg.Genre.Name })
                        .ToListAsync()
                };

                return CreatedAtAction(nameof(GetMovie), new { id = movie.MovieID }, movieDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while creating the movie.");
            }
        }


        // PUT: api/MoviesApi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] UpdateMovieRequestDto updateMovieRequest)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = updateMovieRequest.Title;
            movie.ReleaseDate = updateMovieRequest.ReleaseDate;
            movie.Director = updateMovieRequest.Director;
            movie.Description = updateMovieRequest.Description;
            movie.PosterURL = updateMovieRequest.PosterURL;

            // Remove old genres
            var existingGenres = await _context.MovieGenres.Where(mg => mg.MovieID == id).ToListAsync();
            _context.MovieGenres.RemoveRange(existingGenres);

            // Add new genres
            foreach (var genreId in updateMovieRequest.GenreIds)
            {
                _context.MovieGenres.Add(new MovieGenre { MovieID = movie.MovieID, GenreID = genreId });
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/MoviesApi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/MoviesApi/AddReview
        [HttpPost("AddReview")]
        public async Task<ActionResult<ReviewDto>> AddReview([FromBody] AddReviewRequestDto addReviewRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _context.Movies.FindAsync(addReviewRequest.MovieID);
            if (movie == null)
            {
                return NotFound("Movie not found");
            }

            var review = new Review
            {
                MovieID = addReviewRequest.MovieID,
                UserID = addReviewRequest.UserID,
                Rating = addReviewRequest.Rating,
                ReviewText = addReviewRequest.ReviewText,
                ReviewDate = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            var reviewDto = new ReviewDto
            {
                ReviewID = review.ReviewID,
                MovieID = review.MovieID,
                UserID = review.UserID,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                ReviewDate = review.ReviewDate
            };

            return CreatedAtAction("GetReview", new { id = reviewDto.ReviewID }, reviewDto);
        }

        // GET: api/MoviesApi/GetReview/5
        [HttpGet("GetReview/{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }
            var reviewDto = new ReviewDto
            {
                ReviewID = review.ReviewID,
                MovieID = review.MovieID,
                UserID = review.UserID,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                ReviewDate = review.ReviewDate
            };

            return Ok(reviewDto);
        }


        // PUT: api/MoviesApi/UpdateReview/5
        [HttpPut("UpdateReview/{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] EditReviewRequestDto editReviewRequest)
        {
            if (id != editReviewRequest.ReviewID)
            {
                return BadRequest();
            }

            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            review.Rating = editReviewRequest.Rating;
            review.ReviewText = editReviewRequest.ReviewText;

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/MoviesApi/DeleteReview/5
        [HttpDelete("DeleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewID == id);
        }
    }
}
