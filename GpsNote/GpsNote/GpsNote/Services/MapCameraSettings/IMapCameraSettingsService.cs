using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.MapCameraSettingsService
{
    public interface IMapCameraSettingsService
    {
        void SaveCurrentCameraPositionAsync(CameraPosition cameraPosition);

        CameraPosition GetInitialCameraSettings();
    }
}