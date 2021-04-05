using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.Settings
{
    public class SettingsService : ISettings
    {
        #region -- Implement ISettings --
        public int LoggedUser 
        {
            get => Preferences.Get(nameof(LoggedUser), -1);
            set => Preferences.Set(nameof(LoggedUser), value); 
        }
        public bool DarkTheme 
        { 
            get => Preferences.Get(nameof(DarkTheme), false);
            set => Preferences.Set(nameof(DarkTheme), value);
        }
        public string LastPosition
        {
            get => Preferences.Get(nameof(LastPosition), "");
            set => Preferences.Set(nameof(LastPosition), value);
        }

        public double LastZoom
        {
            get => Preferences.Get(nameof(LastZoom), 2.0);
            set => Preferences.Set(nameof(LastZoom), value);
        }

        public double LastBearing
        { 
            get => Preferences.Get(nameof(LastBearing), 0.0);
            set => Preferences.Set(nameof(LastBearing), value);
        }
        
        public double LastTilt 
        {
            get => Preferences.Get(nameof(LastTilt), 0.0);
            set => Preferences.Set(nameof(LastTilt), value);
        }
        

        #endregion
    }
}