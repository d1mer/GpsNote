using GpsNote.Models;
using System.Threading.Tasks;

namespace GpsNote.Services.Registration
{
    public interface IRegistration
    {
        Task<bool> IsRegistration(User user);
    }
}