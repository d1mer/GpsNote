using GpsNote.Enums;
using GpsNote.Services.SettingsService;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace GpsNote.Services.Color
{
    public class ColorService : IColorService
    {
        private ISettingsManager _settingsManager;

        public ColorService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }


        #region -- IColorService implementation --

        public SKColor GetCurrentDarkColor()
        {
            Colors color = (Colors)_settingsManager.CurrentClockColor;
            SKColor skColor = default(SKColor);

            switch (color)
            {
                case Colors.Red:
                    skColor = Xamarin.Forms.Color.FromHex(Constants.DARK_RED_COLOR).ToSKColor();
                    break;
                case Colors.Green:
                    skColor = Xamarin.Forms.Color.FromHex(Constants.DARK_GREEN_COLOR).ToSKColor();
                    break;
                case Colors.Blue:
                    skColor = Xamarin.Forms.Color.FromHex(Constants.DARK_BLUE_COLOR).ToSKColor();
                    break;
            }

            return skColor;

        }

        public SKColor GetCurrentLightColor()
        {
            Colors color = (Colors)_settingsManager.CurrentClockColor;
            SKColor skColor = default(SKColor);

            switch (color)
            {
                case Colors.Red:
                    skColor = Xamarin.Forms.Color.FromHex(Constants.LIGHT_RED_COLOR).ToSKColor();
                    break;
                case Colors.Green:
                    skColor = Xamarin.Forms.Color.FromHex(Constants.LIGHT_GREEN_COLOR).ToSKColor();
                    break;
                case Colors.Blue:
                    skColor = Xamarin.Forms.Color.FromHex(Constants.LIGHT_BLUE_COLOR).ToSKColor();
                    break;
            }

            return skColor;

        }

        public void SaveCurrentColor(Colors color)
        {
            _settingsManager.CurrentClockColor = (int)color;
        }

        #endregion
    }
}