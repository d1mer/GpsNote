using GpsNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GpsNote.Services.RepositoryService;
using GpsNote.Services.UserService;
using GpsNote.Enums;
using GpsNote.Helpers;

namespace GpsNote.Services.AuthorizationService
{
    public class AuthorizeService : IAuthorizeService
    {
        #region -- Private fields --

        private IUserService _userService;

        #endregion

        public AuthorizeService(IUserService userService)
        {
            _userService = userService;
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

            return CodeUserAuthresult.Passed;
        }
    }
}