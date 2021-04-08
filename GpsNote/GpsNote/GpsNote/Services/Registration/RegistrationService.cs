using GpsNote.Models;
using GpsNote.Services.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GpsNote.Services.Registration
{
    public class RegistrationService : IRegistration
    {
        #region -- Private fields --

        IRepositoryService _repository;

        #endregion

        public RegistrationService(IRepositoryService repository)
        {
            _repository = repository;
        }


        public async Task<bool> IsRegistration(User user)
        {
            User existUser = await _repository.GetEntityAsync<User>((s) => user.Email == s.Email);
            if (existUser != null)
                return false;

            await _repository.InsertAsync(user);
            return true;
        }
    }
}
