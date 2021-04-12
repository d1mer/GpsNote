using GpsNote.Models;
using GpsNote.Services.RepositoryService;
using GpsNote.Services.SettingsService;

namespace GpsNote.Services.UserService
{
    public class UserService : IUserService
    {
        #region -- Private fields --

        IRepositoryService _repositoryService;
        ISettingsService _settingsService;

        #endregion


        #region -- Constructor --

        public UserService(IRepositoryService repositoryService, ISettingsService settingsService)
        {
            _repositoryService = repositoryService;
            _settingsService = settingsService;
        }

        #endregion


        #region -- Implement IUserInterface --

        public bool IsExistUser(string email) =>
            _repositoryService.GetEntityAsync<Models.User>((s) => email == s.Email).Result is Models.User;


        public int SaveNewUser(User user) => _repositoryService.InsertAsync<User>(user).Result;


        public User GetUserByEmail(string email) =>
            _repositoryService.GetEntityAsync<User>((s) => email == s.Email).Result;


        #endregion
    }
}