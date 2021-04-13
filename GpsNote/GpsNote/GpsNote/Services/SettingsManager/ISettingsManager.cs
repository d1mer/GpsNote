using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.SettingsService
{
    public interface ISettingsManager
    {
        int    AuthorizedUserID { get; set; }
        bool       DarkTheme { get; set; }
        double  LastLatitude { get; set; }
        double LastLongitude { get; set; }
        double      LastZoom { get; set; }
        double   LastBearing { get; set; }
        double      LastTilt { get; set; }
        bool         ShowPin { get; set; }
    }
}