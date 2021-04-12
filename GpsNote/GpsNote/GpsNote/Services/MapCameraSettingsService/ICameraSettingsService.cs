using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Services.MapCameraSettingsService
{
    public interface ICameraSettingsService
    {
        void RecordCurrentCameraPositionAsync(CameraPosition cameraPosition);
    }
}