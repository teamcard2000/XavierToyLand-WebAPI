namespace XavierPlayLandAPI.Models.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User? GetUserById(int id);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        bool UsernameExists(string username);
        bool EmailExists(string email);
        void ValidateUser(User user, bool isUpdate = false);
    }
}
