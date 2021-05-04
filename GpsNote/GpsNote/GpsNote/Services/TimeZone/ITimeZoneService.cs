using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Models;

namespace GpsNote.Services.TimeZone
{
    public interface ITimeZoneService
    {
        Task<TimeZoneResponse> GetTimeZoneAsync(Position position); 
    }
}