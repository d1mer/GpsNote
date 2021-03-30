using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

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

        #endregion
    }
}