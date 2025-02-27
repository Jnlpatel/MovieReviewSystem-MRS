using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewSystem.Data;
using MovieReviewSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewSystem.Controllers
{
    public class GenresPageController : Controller
    {
        private readonly MovieReviewSystemContext _context;

        public GenresPageController(MovieReviewSystemContext context)
        {
            _context = context;
        }

        // GET: GenresPage
        public async Task<IActionResult> Index()
        {
            return View(await _context.Genres.Select(g => new GenreDto { GenreID = g.GenreID, Name = g.Name }).ToListAsync());
        }

        // GET: GenresPage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(new GenreDto { GenreID = genre.GenreID, Name = genre.Name });
        }

        // GET: GenresPage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GenresPage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] GenreDto genreDto)
        {
            if (ModelState.IsValid)
            {
                var genre = new Genre { Name = genreDto.Name };
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genreDto);
        }

        // GET: GenresPage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(new GenreDto { GenreID = genre.GenreID, Name = genre.Name });
        }

        // POST: GenresPage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GenreID,Name")] GenreDto genreDto)
        {
            if (id != genreDto.GenreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var genre = await _context.Genres.FindAsync(id);
                    genre.Name = genreDto.Name;
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genreDto.GenreID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(genreDto);
        }

        // GET: GenresPage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(new GenreDto { GenreID = genre.GenreID, Name = genre.Name });
        }

        // POST: GenresPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.GenreID == id);
        }
    }
}
