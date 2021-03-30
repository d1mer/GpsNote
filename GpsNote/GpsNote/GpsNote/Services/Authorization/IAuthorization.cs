using GpsNote.Models;
using System.Threading.Tasks;

namespace GpsNote.Services.Authorization
{
    public interface IAuthorization
    {
        Task<bool> IsAuthorization(User user);
    }
}