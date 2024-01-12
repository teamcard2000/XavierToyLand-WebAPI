using System.Text.RegularExpressions;

namespace XavierPlayLandAPI.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        public bool UsernameExists(string username)
        {
            return TemporaryUsers.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public bool EmailExists(string email)
        {
            return TemporaryUsers.Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void ValidateUser(User user, bool isUpdate = false)
        {
            // Check for special characters and whitespace in username.
            if (!Regex.IsMatch(user.Username, @"^[a-zA-Z0-9]*$"))
            {
                throw new Exception("Username can only contain letters and numbers.");
            }

            // Check if the role is either 'Admin' or 'User'.
            if (!Regex.IsMatch(user.Role, @"\b(Admin|User)\b"))
            {
                throw new Exception("Role must be either 'admin' or 'user'.");
            }

            // When updating, exclude the current user's username and email from the check.
            var anotherUserWithSameUsername = TemporaryUsers.Users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase) && (!isUpdate || u.Id != user.Id));
            var anotherUserWithSameEmail = TemporaryUsers.Users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) && (!isUpdate || u.Id != user.Id));

            if (anotherUserWithSameUsername)
            {
                throw new Exception("Username already exists.");
            }
            if (anotherUserWithSameEmail)
            {
                throw new Exception("Email is already registered.");
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return TemporaryUsers.Users;
        } 

        public User? GetUserById(int id)
        {
            return TemporaryUsers.Users.FirstOrDefault(u => u.Id == id);
        }

        public void CreateUser(User user)
        {
            ValidateUser(user);
            
            user.Id = TemporaryUsers.Users.Max(u => u.Id) + 1; // Auto-increment ID
            TemporaryUsers.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            var existingUser = TemporaryUsers.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                throw new Exception("User does not exist.");
            }

            ValidateUser(user, isUpdate: true);

            // No conflict found, update the user details
            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
        }

        public void DeleteUser(int id)
        {
            var user = TemporaryUsers.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                TemporaryUsers.Users.Remove(user);
            }
        }
    }
}
