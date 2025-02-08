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
    public class MoviesController : ControllerBase
    {
        private readonly MovieReviewSystemContext _context;

        public MoviesController(MovieReviewSystemContext context)
        {
            _context = context;
        }

        // ✅ Get all movies with genres and average rating
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.Reviews)
                .Select(m => new MovieDto
                {
                    MovieID = m.MovieID,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Director = m.Director,
                    Description = m.Description,
                    PosterURL = m.PosterURL,
                    Genres = m.MovieGenres.Select(mg => mg.Genre.Name).ToList(),
                    AverageRating = m.Reviews.Any() ? m.Reviews.Average(r => r.Rating) : 0
                })
                .ToListAsync();

            return Ok(movies);
        }

        // ✅ Get a specific movie by ID with details
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.Reviews)
                .Where(m => m.MovieID == id)
                .Select(m => new MovieDto
                {
                    MovieID = m.MovieID,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Director = m.Director,
                    Description = m.Description,
                    PosterURL = m.PosterURL,
                    Genres = m.MovieGenres.Select(mg => mg.Genre.Name).ToList(),
                    AverageRating = m.Reviews.Any() ? m.Reviews.Average(r => r.Rating) : 0
                })
                .FirstOrDefaultAsync();

            if (movie == null)
                return NotFound("Movie not found");

            return Ok(movie);
        }

        // ✅ Add a new movie
        [HttpPost("Create")]
        public async Task<ActionResult<MovieDto>> CreateMovie(MovieDto movieDto)
        {
            var movie = new Movie
            {
                Title = movieDto.Title,
                ReleaseDate = movieDto.ReleaseDate,
                Director = movieDto.Director,
                Description = movieDto.Description,
                PosterURL = movieDto.PosterURL,
                MovieGenres = new List<MovieGenre>()
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.MovieID }, movieDto);
        }

        // ✅ Update movie details
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MovieDto movieDto)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(m => m.MovieID == id);

            if (movie == null)
                return NotFound("Movie not found");

            movie.Title = movieDto.Title;
            movie.ReleaseDate = movieDto.ReleaseDate;
            movie.Director = movieDto.Director;
            movie.Description = movieDto.Description;
            movie.PosterURL = movieDto.PosterURL;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Delete a movie
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound("Movie not found");

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
