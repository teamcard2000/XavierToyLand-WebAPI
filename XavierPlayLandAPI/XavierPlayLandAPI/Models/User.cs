using System.ComponentModel.DataAnnotations;

namespace XavierPlayLandAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters and numbers.")]
        public string? Username { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        [Required]
        [RegularExpression(@"\b(Admin|User)\b", ErrorMessage = "Role must be either 'Admin' or 'User'.")]
        public string? Role { get; set; }
        public DateTime DateJoined { get; set; }
    }

    public static class TemporaryUsers
    {
        public static List<User> Users = new List<User>
    {
        new User { Id = 1, Username = "admin", Email = "admin@gmail.com", Password = "admin123", Role = "Admin" },
        new User { Id = 2, Username = "user",  Email = "user@gmail.com", Password = "user123", Role = "User" }
    };
    }
}
