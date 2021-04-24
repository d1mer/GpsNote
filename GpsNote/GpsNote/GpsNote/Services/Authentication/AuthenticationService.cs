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
        private IAuthorizationService _authorizationService;
        private IRepositoryService _repositoryService;
        private ISettingsManager _settingsManager;


        public AuthenticationService(IAuthorizationService authorizationService, 
                                     IRepositoryService repositoryService, 
                                     ISettingsManager settingsManager)
        {
            _authorizationService = authorizationService;
            _repositoryService = repositoryService;
            _settingsManager = settingsManager;
        }



        #region -- IAuthenticationService implementation --

        public async Task<(CodeUserAuthresult, CodeUserAuthresult)> SignInAsync(string email, string password)
        {
            CodeUserAuthresult resultEmail = CodeUserAuthresult.Undefined;
            CodeUserAuthresult resultPassword = CodeUserAuthresult.Undefined;

            email = email.Trim();
            password = password.Trim();

            if (!Validator.Validate(email, Validator.patternEmail))
            {
                resultEmail = CodeUserAuthresult.InvalidEmail;
            }

            if (!Validator.Validate(password, Validator.patternPassword))
            {
                resultPassword = CodeUserAuthresult.InvalidPassword;
            }

            if(resultEmail == CodeUserAuthresult.Undefined && resultPassword == CodeUserAuthresult.Undefined)
            {
                User findUser = null;

                try
                {
                    findUser = await _repositoryService.FindEntity<User>(u => u.Email == email);
                }
                catch (Exception ex)
                {
                    resultEmail = CodeUserAuthresult.UnknownError;
                    resultPassword = CodeUserAuthresult.UnknownError;
                    Console.WriteLine(ex.Message);
                }

                if(findUser != null)
                {
                    resultEmail = CodeUserAuthresult.Passed;

                    if(findUser.Password == password)
                    {
                        _settingsManager.AuthorizedUserID = findUser.Id;
                        resultEmail = CodeUserAuthresult.Passed;
                        resultPassword = CodeUserAuthresult.Passed;
                        _settingsManager.AuthorizedUserID = findUser.Id;
                    }
                    else
                    {
                        resultPassword = CodeUserAuthresult.WrongPassword;
                    }
                }
                else
                {
                    resultEmail = CodeUserAuthresult.EmailNotFound;
                }
            }

            return (resultEmail, resultPassword);
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
                    Console.WriteLine(ex.Message);
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