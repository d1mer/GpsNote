using System.Threading.Tasks;
using GpsNote.Enums;

namespace GpsNote.Services.AuthorizeService
{
    public interface IAuthorizeService
    {
        int IdCurrentUser { get; }

        Task<CodeUserAuthresult> Authorize(string email, string password);

        bool IsAuthorize();
    }
}