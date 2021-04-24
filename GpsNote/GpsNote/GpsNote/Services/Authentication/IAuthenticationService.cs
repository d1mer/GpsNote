using System.Threading.Tasks;
using GpsNote.Enums;

namespace GpsNote.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> OnSignUpAsync(string name, string email, string password);

        Task<(CodeUserAuthresult, CodeUserAuthresult)> OnSignInAsync(string email, string password);

        Task<bool> OnEmailTakenAsync(string email);
    }
}