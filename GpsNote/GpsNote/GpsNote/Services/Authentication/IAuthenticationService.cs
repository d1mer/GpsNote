using System.Threading.Tasks;
using GpsNote.Enums;

namespace GpsNote.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<CodeUserAuthresult> SignUpAsync(string name, string email, string password, string confirmPassword);

        Task<(CodeUserAuthresult, CodeUserAuthresult)> SignInAsync(string email, string password);
    }
}