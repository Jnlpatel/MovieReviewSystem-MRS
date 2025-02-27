using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewSystem.Data;
using MovieReviewSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewSystem.Controllers
{
    public class MoviesPageController : Controller
    {
        private readonly MovieReviewSystemContext _context;

        public MoviesPageController(MovieReviewSystemContext context)
        {
            _context = context;
        }

        // GET: MoviesPage
        // GET: MoviesPage
        public async Task<IActionResult> Index()
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
        // GET: MoviesPage/Delete/5
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

        // POST: MoviesPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        // GET: MoviesPage/EditReview/5
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

            var editReviewRequest = new EditReviewRequestDto
            {
                ReviewID = review.ReviewID,
                MovieID = review.MovieID,
                UserID = review.UserID,
                Rating = review.Rating,
                ReviewText = review.ReviewText
            };

            return View(editReviewRequest);
        }

        // POST: MoviesPage/EditReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReview(int id, EditReviewRequestDto editReviewRequest)
        {
            if (id != editReviewRequest.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var review = await _context.Reviews.FindAsync(id);
                    if (review == null)
                    {
                        return NotFound();
                    }

                    review.Rating = editReviewRequest.Rating;
                    review.ReviewText = editReviewRequest.ReviewText;

                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(editReviewRequest.ReviewID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Reviews), new { id = editReviewRequest.MovieID });
            }
            return View(editReviewRequest);
        }

        // GET: MoviesPage/DeleteReview/5
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

            var deleteReviewRequest = new DeleteReviewRequestDto
            {
                ReviewID = review.ReviewID,
                MovieID = review.MovieID,
                UserID = review.UserID,
                Rating = review.Rating,
                ReviewText = review.ReviewText
            };

            return View(deleteReviewRequest);
        }

        // POST: MoviesPage/DeleteReview/5
        [HttpPost, ActionName("DeleteReview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReviewConfirmed(int id, DeleteReviewRequestDto deleteReviewRequest)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Reviews), new { id = deleteReviewRequest.MovieID });
        }

        // GET: MoviesPage/AddReview/5
        public IActionResult AddReview(int id)
        {
            var addReviewRequest = new AddReviewRequestDto
            {
                MovieID = id
            };
            return View(addReviewRequest);
        }

        // POST: MoviesPage/AddReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(AddReviewRequestDto addReviewRequest)
        {
            if (ModelState.IsValid)
            {
                var review = new Review
                {
                    MovieID = addReviewRequest.MovieID,
                    UserID = 1,
                    Rating = addReviewRequest.Rating,
                    ReviewText = addReviewRequest.ReviewText,
                    ReviewDate = DateTime.Now
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Reviews), new { id = addReviewRequest.MovieID });
            }

            return View(addReviewRequest);
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
