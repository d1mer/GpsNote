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

        public async Task<(CodeUserAuthresult, CodeUserAuthresult)> OnSignInAsync(string email, string password)
        {
            CodeUserAuthresult resultEmail = CodeUserAuthresult.Undefined;
            CodeUserAuthresult resultPassword = CodeUserAuthresult.Undefined;

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

            if (findUser != null)
            {
                resultEmail = CodeUserAuthresult.Passed;

                if (findUser.Password == password)
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

            return (resultEmail, resultPassword);
        }


        public async Task<bool> OnSignUpAsync(string name, string email, string password)
        {
            bool result = false;

            User user = new User
            {
                Name = name,
                Email = email,
                Password = password
            };

            int rows = await _repositoryService.InsertAsync<User>(user);

            if (rows == 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }

        public async Task<bool> OnEmailTakenAsync(string email)
        {
            bool result = false;
            User findUser = null;

            try
            {
                findUser = await _repositoryService.FindEntity<User>(u => u.Email == email);
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine(ex.Message);
            }

            if(findUser == null)
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }

        #endregion
    }
}