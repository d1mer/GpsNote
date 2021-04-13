using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Services.SettingsService;

namespace GpsNote.Services.MapCameraSettingsService
{
    public class CameraSettingsService : ICameraSettingsService
    {
        #region -- Private --

        private readonly ISettingsManager _settingsService;

        #endregion


        #region -- Constructor --

        public CameraSettingsService(ISettingsManager settingsService)
        {
            _settingsService = settingsService;
        }

        #endregion


        public async void RecordCurrentCameraPositionAsync(CameraPosition cameraPosition)
        {
            await Task.Run(() => SaveCameraPosition(cameraPosition));
        }


        public CameraPosition GetInitialCameraSettings()
        {
            CameraPosition cameraPosition = null;

            if (_settingsService.LastZoom != 0.0)
            {
                Position position = new Position(_settingsService.LastLatitude, _settingsService.LastLongitude);

                cameraPosition = new CameraPosition(position,
                                                    _settingsService.LastZoom,
                                                    _settingsService.LastBearing,
                                                    _settingsService.LastTilt);
            }

            return cameraPosition;
        }


        #region -- Private helpers --

        private void SaveCameraPosition(CameraPosition cameraPosition)
        {
            _settingsService.LastLatitude = cameraPosition.Target.Latitude;
            _settingsService.LastLongitude = cameraPosition.Target.Longitude;
            _settingsService.LastZoom = cameraPosition.Zoom;
            _settingsService.LastBearing = cameraPosition.Bearing;
            _settingsService.LastTilt = cameraPosition.Tilt;
        }

        #endregion
    }
}