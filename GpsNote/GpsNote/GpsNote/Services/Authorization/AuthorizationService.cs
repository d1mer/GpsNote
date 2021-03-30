using GpsNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GpsNote.Services.Repository;

namespace GpsNote.Services.Authorization
{
    public class AuthorizationService : IAuthorization
    {
        #region -- Private fields --

        IRepository _repository;

        #endregion

        public AuthorizationService(IRepository repository)
        {
            _repository = repository;
        }


        public async Task<bool> IsAuthorization(User user)
        {
            User existUser = await _repository.GetEntityAsync<User>((s) => user.Email == s.Email);
            if (existUser == null)
                return false;
            return user.Password == existUser.Password;
        }
    }
}