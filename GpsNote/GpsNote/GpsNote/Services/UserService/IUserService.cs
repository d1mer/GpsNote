using GpsNote.Models;

namespace GpsNote.Services.UserService
{
    public interface IUserService
    {
        bool IsExistUser(string email);

        int SaveNewUser(User user);

        User GetUserByEmail(string email);
    }
}