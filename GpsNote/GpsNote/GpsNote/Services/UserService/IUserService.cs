using System;
using System.Collections.Generic;
using System.Text;
using GpsNote.Models;
using GpsNote.Services.RepositoryService;

namespace GpsNote.Services.UserService
{
    public interface IUserService
    {
        bool IsExistUser(string email);

        int SaveNewUser(User user);

        User GetUserByEmail(string email);

        void
    }
}