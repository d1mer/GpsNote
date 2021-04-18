using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Services.SettingsService;
using GpsNote;

namespace GpsNote.Services.MapCameraSettingsService
{
    public class MapCameraSettingsService : IMapCameraSettingsService
    {
        private readonly ISettingsManager _settingsService;

        public MapCameraSettingsService(ISettingsManager settingsService)
        {
            _settingsService = settingsService;
        }

        #region -- IMapCameraSettingsService implementation --

        public async void SaveCurrentCameraPositionAsync(CameraPosition cameraPosition)
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
            else
            {
                cameraPosition = new CameraPosition(new Position(0, 0), Constants.INITIAL_ZOOM);
            }

            return cameraPosition;
        }

        #endregion


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