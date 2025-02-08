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
    public class GenreController : ControllerBase
    {
        private readonly MovieReviewSystemContext _context;

        public GenreController(MovieReviewSystemContext context)
        {
            _context = context;
        }

        // ✅ Get all genres
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
        {
            var genres = await _context.Genres
                .Select(g => new GenreDto
                {
                    GenreID = g.GenreID,
                    Name = g.Name
                })
                .ToListAsync();

            return Ok(genres);
        }

        // ✅ Get a specific genre by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDto>> GetGenre(int id)
        {
            var genre = await _context.Genres
                .Where(g => g.GenreID == id)
                .Select(g => new GenreDto
                {
                    GenreID = g.GenreID,
                    Name = g.Name
                })
                .FirstOrDefaultAsync();

            if (genre == null)
                return NotFound("Genre not found");

            return Ok(genre);
        }

        // ✅ Add a new genre
        [HttpPost("Add")]
        public async Task<ActionResult<GenreDto>> AddGenre(GenreDto genreDto)
        {
            if (string.IsNullOrEmpty(genreDto.Name))
                return BadRequest("Genre name is required");

            var genre = new Genre
            {
                Name = genreDto.Name
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            genreDto.GenreID = genre.GenreID; // Set the newly created ID

            return CreatedAtAction(nameof(GetGenre), new { id = genre.GenreID }, genreDto);
        }

        // ✅ Update an existing genre
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateGenre(int id, GenreDto genreDto)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
                return NotFound("Genre not found");

            genre.Name = genreDto.Name;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Delete a genre
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
                return NotFound("Genre not found");

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
