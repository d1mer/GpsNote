using System;
using System.Threading.Tasks;
using GpsNote.Enums;
using GpsNote.Helpers;
using GpsNote.Models;
using GpsNote.Services.Authorization;
using GpsNote.Services.RepositoryService;
using GpsNote.Services.SettingsService;

namespace GpsNote.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        #region -- Private fields --

        IAuthorizationService _authorizationService;
        IRepositoryService _repositoryService;
        ISettingsManager _settingsManager;

        #endregion


        #region -- Constructor --

        public AuthenticationService(IAuthorizationService authorizationService, IRepositoryService repositoryService, ISettingsManager settingsManager)
        {
            _authorizationService = authorizationService;
            _repositoryService = repositoryService;
            _settingsManager = settingsManager;
        }

        #endregion



        #region -- IAuthenticationService implementation --

        public async Task<CodeUserAuthresult> SignInAsync(string email, string password)
        {
            CodeUserAuthresult result = CodeUserAuthresult.Passed;

            if (!Validator.Validate(email, Validator.patternEmail))
            {
                result = CodeUserAuthresult.InvalidEmail;
            }

            if (!Validator.Validate(password, Validator.patternPassword))
            {
                result = CodeUserAuthresult.InvalidPassword;
            }

            if(result == CodeUserAuthresult.Passed)
            {
                User findUser = null;

                try
                {
                    findUser = await _repositoryService.FindEntity<User>(u => u.Email == email);
                }
                catch (Exception ex)
                {
                    result = CodeUserAuthresult.UnknownError;
                }

                if(findUser != null)
                {
                    if(findUser.Password == password)
                    {
                        _settingsManager.AuthorizedUserID = findUser.Id;
                    }
                    else
                    {
                        result = CodeUserAuthresult.WrongPassword;
                    }
                }
                else
                {
                    result = CodeUserAuthresult.EmailNotFound;
                }
            }

            return result;
        }

        public async Task<CodeUserAuthresult> SignUpAsync(string name, string email, string password, string confirmPassword)
        {
            CodeUserAuthresult result = CodeUserAuthresult.Passed;

            name = name.Trim();
            email = email.Trim();
            password = password.Trim();
            confirmPassword = confirmPassword.Trim();

            if (!Validator.Validate(name, Validator.patternName))
            {
                result = CodeUserAuthresult.InvalidName;
            }

            if (!Validator.Validate(email, Validator.patternEmail))
            {
                result = CodeUserAuthresult.InvalidEmail;
            }

            if (!Validator.Validate(password, Validator.patternPassword))
            {
                result = CodeUserAuthresult.InvalidPassword;
            }

            if (password != confirmPassword)
            {
                result = CodeUserAuthresult.PasswordMismatch;
            }

            if(result == CodeUserAuthresult.Passed)
            {
                User findUser = null;

                try
                {
                    findUser = await _repositoryService.FindEntity<User>(u => u.Email == email);
                }
                catch (Exception ex)
                {
                    result = CodeUserAuthresult.UnknownError;
                }

                if (findUser == null)
                {
                    findUser = new User
                    {
                        Name = name,
                        Email = email,
                        Password = password
                    };

                    int rows = await _repositoryService.InsertAsync<User>(findUser);

                    if(rows == 0)
                    {
                        result = CodeUserAuthresult.UnknownError;
                    }
                }
                else
                {
                    result = CodeUserAuthresult.EmailTaken;
                }
            }

            return result;
        }

        #endregion
    }
}