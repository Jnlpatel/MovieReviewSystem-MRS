namespace MovieReviewSystem.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public int MovieID { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public User User { get; set; }
        public Movie Movie { get; set; }
    }
    public class ReviewDto
    {
        public int ReviewID { get; set; }
        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
