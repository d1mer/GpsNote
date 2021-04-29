using GpsNote.Services.SettingsService;

namespace GpsNote.Services.Theme
{
    public class ThemeService : IThemeService
    {
        ISettingsManager _settingsManager;

        public ThemeService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public int Theme 
        {
            get => _settingsManager.DarkTheme ? 2 : 1;
            set => _settingsManager.DarkTheme = value == 2 ? true : false;
        }
    }
}