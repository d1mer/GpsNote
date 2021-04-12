using GpsNote.Services.SettingsService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.MapCameraSettingsService
{
    public class CameraSettingsService : ICameraSettingsService
    {
        #region -- Private --

        ISettingsService _settingsService;

        #endregion


        #region -- Constructor --

        public CameraSettingsService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        #endregion


        public async void RecordCurrentCameraPositionAsync(CameraPosition cameraPosition)
        {
            await Task.Run(() => SaveCameraPosition(cameraPosition));
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