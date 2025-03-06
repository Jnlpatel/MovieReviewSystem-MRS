using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRS.Models;
using MRS.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;

namespace MRS.Controllers
{
    public class MoviesPageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesPageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .ToListAsync();

            return View(movies);
        }

        // GET: MoviesPage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Include(m => m.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.MovieID == id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = new MovieDto
            {
                MovieID = movie.MovieID,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Director = movie.Director,
                Description = movie.Description,
                PosterURL = movie.PosterURL,
                AverageRating = movie.Reviews.Any() ? movie.Reviews.Average(r => r.Rating) : 0,
                Genres = movie.MovieGenres.Select(mg => new GenreDto { GenreID = mg.Genre.GenreID, Name = mg.Genre.Name }).ToList()
            };

            return View(movieDto);
        }

        // GET: MoviesPage/Create
        public IActionResult Create()
        {
            ViewBag.Genres = _context.Genres.Select(g => new GenreDto { GenreID = g.GenreID, Name = g.Name }).ToList();
            return View();
        }

        // POST: MoviesPage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(CreateMovieRequestDto createMovieRequest)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movie
                {
                    Title = createMovieRequest.Title,
                    ReleaseDate = createMovieRequest.ReleaseDate,
                    Director = createMovieRequest.Director,
                    Description = createMovieRequest.Description,
                    PosterURL = createMovieRequest.PosterURL
                };

                _context.Add(movie);
                await _context.SaveChangesAsync();

                foreach (var genreId in createMovieRequest.GenreIds)
                {
                    _context.MovieGenres.Add(new MovieGenre { MovieID = movie.MovieID, GenreID = genreId });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Genres = _context.Genres.Select(g => new GenreDto { GenreID = g.GenreID, Name = g.Name }).ToList();
            return View(createMovieRequest);
        }

        // GET: MoviesPage/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(m => m.MovieID == id);

            if (movie == null)
            {
                return NotFound();
            }

            var updateMovieRequest = new UpdateMovieRequestDto
            {
                MovieID = movie.MovieID,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Director = movie.Director,
                Description = movie.Description,
                PosterURL = movie.PosterURL,
                GenreIds = movie.MovieGenres.Select(mg => mg.GenreID).ToList()
            };

            ViewBag.Genres = _context.Genres.Select(g => new GenreDto { GenreID = g.GenreID, Name = g.Name }).ToList();
            return View(updateMovieRequest);
        }

        // POST: MoviesPage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Edit(int id, UpdateMovieRequestDto updateMovieRequest)
        {
            if (id != updateMovieRequest.MovieID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
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

                    _context.Update(movie);

                    var existingGenres = await _context.MovieGenres.Where(mg => mg.MovieID == id).ToListAsync();
                    _context.MovieGenres.RemoveRange(existingGenres);

                    foreach (var genreId in updateMovieRequest.GenreIds)
                    {
                        _context.MovieGenres.Add(new MovieGenre { MovieID = movie.MovieID, GenreID = genreId });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(updateMovieRequest.MovieID))
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
            ViewBag.Genres = _context.Genres.Select(g => new GenreDto { GenreID = g.GenreID, Name = g.Name }).ToList();
            return View(updateMovieRequest);
        }

        // GET: Movies/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/AddReview/5
        [Authorize]
        public async Task<IActionResult> AddReview(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var addReviewRequest = new AddReviewRequestDto
            {
                MovieID = movie.MovieID,
                UserID = userId
            };

            return View(addReviewRequest);
        }

        // POST: Movies/AddReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddReview([Bind("MovieID,UserID,Rating,ReviewText")] AddReviewRequestDto addReviewRequest)
        {
            if (ModelState.IsValid)
            {
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

                return RedirectToAction(nameof(Details), new { id = addReviewRequest.MovieID });
            }

            return View(addReviewRequest);
        }

        // GET: Movies/EditReview/5
        [Authorize]
        public async Task<IActionResult> EditReview(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Movies/EditReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditReview(int id, [Bind("ReviewID,MovieID,UserID,Rating,ReviewDate,ReviewText")] Review review)
        {
            if (id != review.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = review.MovieID });
            }
            return View(review);
        }

        // GET: Movies/DeleteReview/5
        [Authorize]
        public async Task<IActionResult> DeleteReview(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Movies/DeleteReview/5
        [HttpPost, ActionName("DeleteReview")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteReviewConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            var movieId = review.MovieID; // Store MovieID before deleting the review
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = movieId });
        }


        // GET: MoviesPage/Reviews/5
        public async Task<IActionResult> Reviews(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewBag.MovieTitle = movie.Title;
            ViewBag.MovieID = id;

            var reviews = await _context.Reviews
                .Where(r => r.MovieID == id)
                .Select(r => new ReviewDto
                {
                    ReviewID = r.ReviewID,
                    MovieID = r.MovieID,
                    UserID = r.UserID,
                    Rating = r.Rating,
                    ReviewText = r.ReviewText,
                    ReviewDate = r.ReviewDate
                })
                .ToListAsync();

            return View(reviews);
        }
        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieID == id);
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewID == id);
        }
    }
}
