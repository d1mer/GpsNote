using Xamarin.Essentials;

namespace GpsNote.Services.SettingsService
{
    public class SettingsManager : ISettingsManager
    {
        #region -- Implement ISettings --

        public int AuthorizedUserID 
        {
            get => Preferences.Get(nameof(AuthorizedUserID), -1);
            set => Preferences.Set(nameof(AuthorizedUserID), value); 
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
            get => Preferences.Get(nameof(LastZoom), default(double));
            set => Preferences.Set(nameof(LastZoom), value);
        }

        public double LastBearing
        { 
            get => Preferences.Get(nameof(LastBearing), default(double));
            set => Preferences.Set(nameof(LastBearing), value);
        }
        
        public double LastTilt 
        {
            get => Preferences.Get(nameof(LastTilt), default(double));
            set => Preferences.Set(nameof(LastTilt), value);
        }


        public bool ShowPin
        {
            get => Preferences.Get(nameof(ShowPin), false);
            set => Preferences.Set(nameof(ShowPin), value);
        }

        public bool LocationPermission
        {
            get => Preferences.Get(nameof(LocationPermission), false);
            set => Preferences.Set(nameof(LocationPermission), value);
        }

        public void CleanUpAuthorizedUser()
        {
            AuthorizedUserID = -1;
            DarkTheme = false;
            LastLatitude = default(double);
            LastLongitude = default(double);
            LastZoom = default(double);
            LastBearing = default(double);
            LastTilt = default(double);
            LocationPermission = false;
        }

        #endregion
    }
}