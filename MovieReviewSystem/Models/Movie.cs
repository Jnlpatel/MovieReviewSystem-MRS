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
        public ICollection<Review> Reviews { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
    public class MovieDto
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public string PosterURL { get; set; }
        public List<string> Genres { get; set; }  // Genre names
        public double AverageRating { get; set; }
    }


}
