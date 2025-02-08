namespace MovieReviewSystem.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

    // DTO for API responses
    public class UserDto
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
