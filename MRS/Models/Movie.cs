using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MRS.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }

        [Required(ErrorMessage = "The Title field is required.")]
        [StringLength(255, ErrorMessage = "The Title cannot exceed 255 characters.")]
        [Display(Name = "Movie Title")]
        public string? Title { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The Release Date field is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
        public string PosterURL { get; set; }
        public string Director { get; set; }

        // Navigation properties:
        // One-to-many relationship with Reviews
        public ICollection<Review>? Reviews { get; set; }

        // Many-to-many relationship with Genres (through MovieGenres)
        public ICollection<MovieGenre>? MovieGenres { get; set; }
    }
    public class MovieDto
    {
        public int MovieID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterURL { get; set; }
        public string Director { get; set; }
        public double AverageRating { get; set; }
        public List<GenreDto>? Genres { get; set; }
    }
    public class CreateMovieRequestDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string Director { get; set; }

        public string Description { get; set; }

        public string PosterURL { get; set; }

        public List<int>? GenreIds { get; set; } // For selecting multiple genres
    }
    public class UpdateMovieRequestDto
    {
        [Required]
        public int MovieID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string Director { get; set; }

        public string Description { get; set; }

        public string PosterURL { get; set; }

        public List<int>? GenreIds { get; set; } // For selecting multiple genres
    }
    public class AddReviewRequestDto
    {
        [Required]
        public int MovieID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string ReviewText { get; set; }
    }

    public class EditReviewRequestDto
    {
        [Required]
        public int ReviewID { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        public string ReviewText { get; set; }
    }
}
