using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.Settings
{
    public interface ISettings
    {
        int LoggedUser { get; set; }
        bool DarkTheme { get; set; }
        Position LastPosition { get; set; }
    }
}