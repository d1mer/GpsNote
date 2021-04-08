using GpsNote.Enums;
using GpsNote.Helpers;
using GpsNote.Models;
using GpsNote.Services.RepositoryService;
using GpsNote.Services.UserService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GpsNote.Services.RegistrationService
{
    public class RegistrationService : IRegistrationService
    {
        #region -- Private fields --

        IUserService _userService;

        #endregion

        public RegistrationService(IUserService userService)
        {
            _userService = userService;
        }


        public async Task<CodeUserAuthresult> IsRegistration(string name, string email, string password, string confirmPassword)
        {
            name = name.Trim();
            email = email.Trim();
            password = password.Trim();
            confirmPassword = confirmPassword.Trim();

            if (!Validator.Validate(name, Validator.patternName))
            {
                return CodeUserAuthresult.InvalidName;
            }

            if (!Validator.Validate(email, Validator.patternEmail))
            {
                return CodeUserAuthresult.InvalidEmail;
            }

            if (!Validator.Validate(password, Validator.patternPassword))
            {
                return CodeUserAuthresult.InvalidPassword;
            }

            if (password != confirmPassword)
            {
                return CodeUserAuthresult.PasswordMismatch;
            }

            if (_userService.IsExistUser(email))
            {
                return CodeUserAuthresult.EmailTaken;
            }

            User user = new User
            {
                Name = name,
                Email = email,
                Password = password
            };

            try
            {
                _userService.SaveNewUser(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return CodeUserAuthresult.Passed;
        }
    }
}
