namespace GpsNote.Services.Localization
{
    public interface ILocalizationService
    {
        string this[string key]
        {
            get;
        }

        string Lang { get; set; }

        void ChangeCulture(string lang);
    }
}