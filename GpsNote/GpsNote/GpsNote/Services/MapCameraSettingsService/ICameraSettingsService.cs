using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.MapCameraSettingsService
{
    public interface ICameraSettingsService
    {
        void RecordCurrentCameraPositionAsync(CameraPosition cameraPosition);

        CameraPosition GetInitialCameraSettings();
    }
}