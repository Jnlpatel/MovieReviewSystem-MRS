namespace MovieReviewSystem.Models
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public string PosterURL { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<MovieGenre>? MovieGenres { get; set; } = new List<MovieGenre>();

    }
    public class MovieDto
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public string PosterURL { get; set; }
        
        public double AverageRating { get; set; }
        public List<GenreDto> Genres { get; set; }

    }
    public class CreateMovieRequestDto
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public string PosterURL { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();
    }

    public class UpdateMovieRequestDto
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public string PosterURL { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();

    }

    public class AddReviewRequestDto
    {
        public int MovieID { get; set; }  // Add this line
        public int UserID { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
    }

}
