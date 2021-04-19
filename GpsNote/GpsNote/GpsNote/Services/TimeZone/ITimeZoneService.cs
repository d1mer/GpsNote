using GpsNote.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.TimeZone
{
    public interface ITimeZoneService
    {
        Task<TimeZoneResponse> GetTimeZoneAsync(Position position); 
    }
}