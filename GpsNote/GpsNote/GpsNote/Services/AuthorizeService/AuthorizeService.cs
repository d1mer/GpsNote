using GpsNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GpsNote.Services.RepositoryService;
using GpsNote.Services.UserService;
using GpsNote.Enums;
using GpsNote.Helpers;
using GpsNote.Services.SettingsService;

namespace GpsNote.Services.AuthorizeService
{
    public class AuthorizeService : IAuthorizeService
    {
        #region -- Private fields --

        private IUserService _userService;
        private ISettingsService _settingsService;

        #endregion


        #region -- Constructor --

        public AuthorizeService(IUserService userService, ISettingsService settingsService)
        {
            _userService = userService;
            _settingsService = settingsService;
        }

        #endregion


        #region -- Inplement IAuthorizeService interface --

        public int IdCurrentUser
        {
            get => _settingsService.IdCurrentUser;
        }

        public async Task<CodeUserAuthresult> Authorize(string email, string password)
        {
            if (!Validator.Validate(email, Validator.patternEmail))
            {
                return CodeUserAuthresult.InvalidEmail;
            }

            if (!Validator.Validate(password, Validator.patternPassword))
            {
                return CodeUserAuthresult.InvalidPassword;
            }


            User user = _userService.GetUserByEmail(email);

            if (user == null)
                return CodeUserAuthresult.EmailNotFound;

            if (user.Password != password)
            {
                return CodeUserAuthresult.WrongPassword;
            }

            _userService.SaveIdCurrentUser(user.Id);

            return CodeUserAuthresult.Passed;
        }

        #endregion
    }
}