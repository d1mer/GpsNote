namespace GpsNote.Services.SettingsService
{
    public interface ISettingsManager
    {
        int AuthorizedUserID { get; set; }
        bool DarkTheme { get; set; }
        double LastLatitude { get; set; }
        double LastLongitude { get; set; }
        double LastZoom { get; set; }
        double LastBearing { get; set; }
        double LastTilt { get; set; }
        bool LocationPermission { get; set; }
        string Lang { get; set; }

        int CurrentClockColor { get; set; }
        void CleanUpAuthorizedUser();
    }
}