namespace MovieReviewSystem.Models
{
    public class Genre
    {
        public int GenreID { get; set; }
        public string Name { get; set; }
        public ICollection<MovieGenre>? MovieGenres { get; set; } = new List<MovieGenre>();


    }
    public class GenreDto
    {
        public int GenreID { get; set; }
        public string Name { get; set; }
    }

}
