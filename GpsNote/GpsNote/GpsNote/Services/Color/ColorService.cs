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
                    skColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["darkRedClock"] as string).ToSKColor();
                    break;
                case Colors.Green:
                    skColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["darkGreenClock"] as string).ToSKColor();
                    break;
                case Colors.Blue:
                    skColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["darkBlueClock"] as string).ToSKColor();
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
                    skColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["lightRedClock"] as string).ToSKColor();
                    break;
                case Colors.Green:
                    skColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["lightGreenClock"] as string).ToSKColor();
                    break;
                case Colors.Blue:
                    skColor = Xamarin.Forms.Color.FromHex(App.Current.Resources["lightBlueClock"] as string).ToSKColor();
                    break;
            }

            return skColor;

        }

        public void SaveCurrentColor(Colors color)
        {
            _settingsManager.CurrentClockColor = (int)color;
        }

        public int GetCurrentColor()
        {
            return _settingsManager.CurrentClockColor;
        }

        public bool IsDarkTheme()
        {
            return _settingsManager.DarkTheme;
        }

        #endregion
    }
}