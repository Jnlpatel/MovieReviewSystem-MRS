using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRS.Data;
using MRS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GenresApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
        {
            var genres = await _context.Genres
                .Select(g => new GenreDto { GenreID = g.GenreID, Name = g.Name })
                .ToListAsync();

            return Ok(genres);
        }

        // GET: api/GenresApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDto>> GetGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            var genreDto = new GenreDto { GenreID = genre.GenreID, Name = genre.Name };
            return Ok(genreDto);
        }

        // PUT: api/GenresApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, GenreDto genreDto)
        {
            if (id != genreDto.GenreID)
            {
                return BadRequest();
            }

            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            genre.Name = genreDto.Name;

            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/GenresApi
        [HttpPost]
        public async Task<ActionResult<GenreDto>> PostGenre(GenreDto genreDto)
        {
            var genre = new Genre { Name = genreDto.Name };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            genreDto.GenreID = genre.GenreID; // update genreDto with the new ID

            return CreatedAtAction("GetGenre", new { id = genre.GenreID }, genreDto);
        }

        // DELETE: api/GenresApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.GenreID == id);
        }
    }
}
