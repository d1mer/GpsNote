using GpsNote.Models;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Extensions
{
    public static class PinExtension
    {
        public static PinModel PinToPinModel(this Pin pin)
        {
            PinModel pinModel = new PinModel
            {
                Latitude = pin.Position.Latitude,
                Longitude = pin.Position.Longitude,
                Label = pin.Label,
                Address = pin.Address,
            };

            return pinModel;
        }
    }
}
