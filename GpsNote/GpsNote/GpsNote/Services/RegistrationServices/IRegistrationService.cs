using GpsNote.Enums;
using System.Threading.Tasks;

namespace GpsNote.Services.RegistrationService
{
    public interface IRegistrationService
    {
        Task<CodeUserAuthresult> IsRegistration(string name, string email, string password, string confirmPassword);
    }
}