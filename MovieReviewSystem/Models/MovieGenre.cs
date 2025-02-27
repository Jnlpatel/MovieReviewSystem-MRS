using System.Text.Json.Serialization;
namespace MovieReviewSystem.Models
{
    public class MovieGenre
    {
        public int MovieID { get; set; }
        [JsonIgnore]
        public Movie Movie { get; set; }
        public int GenreID { get; set; }
        public Genre Genre { get; set; }
    }
}
