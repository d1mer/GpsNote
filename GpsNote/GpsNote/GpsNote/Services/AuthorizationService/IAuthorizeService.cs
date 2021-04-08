using System.Threading.Tasks;
using GpsNote.Enums;

namespace GpsNote.Services.AuthorizationService
{
    public interface IAuthorizeService
    {
        Task<CodeUserAuthresult> Authorize(string email, string password);
    }
}