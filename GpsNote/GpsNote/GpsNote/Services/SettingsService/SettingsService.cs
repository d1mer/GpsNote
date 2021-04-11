using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.SettingsService
{
    public class SettingsService : ISettingsService
    {
        #region -- Implement ISettings --

        public int IdCurrentUser 
        {
            get => Preferences.Get(nameof(IdCurrentUser), -1);
            set => Preferences.Set(nameof(IdCurrentUser), value); 
        }
        public bool DarkTheme 
        { 
            get => Preferences.Get(nameof(DarkTheme), false);
            set => Preferences.Set(nameof(DarkTheme), value);
        }
        public double LastLatitude
        {
            get => Preferences.Get(nameof(LastLatitude), default(double));
            set => Preferences.Set(nameof(LastLatitude), value);
        }

        public double LastLongitude 
        {
            get => Preferences.Get(nameof(LastLongitude), default(double));
            set => Preferences.Set(nameof(LastLongitude), value);
        }

        public double LastZoom
        {
            get => Preferences.Get(nameof(LastZoom), 0.0);
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


        public bool ShowPin
        {
            get => Preferences.Get(nameof(ShowPin), false);
            set => Preferences.Set(nameof(ShowPin), value);
        }
        

        #endregion
    }
}