using GpsNote.Models;
using GpsNote.Services.RepositoryService;

namespace GpsNote.Services.UserService
{
    public class UserService : IUserService
    {
        #region -- Private fields --

        IRepositoryService _repositoryService;

        #endregion


        #region -- Implement IUserInterface --

        public UserService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public bool IsExistUser(string email) =>
            _repositoryService.GetEntityAsync<Models.User>((s) => email == s.Email).Result is Models.User;


        public int SaveNewUser(User user) => _repositoryService.InsertAsync<User>(user).Result;


        public User GetUserByEmail(string email) =>
            _repositoryService.GetEntityAsync<User>((s) => email == s.Email).Result;

        #endregion
    }
}