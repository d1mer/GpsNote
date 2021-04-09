using GpsNote.Models;
using GpsNote.ViewModels.ExtentedViewModels;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Extensions
{
    public static class PinExtensions
    {
        public static PinModelDb PinToPinModelDb(this Pin pin)
        {
            PinModelDb pinModelDb = new PinModelDb
            {
                Latitude = pin.Position.Latitude,
                Longitude = pin.Position.Longitude,
                Label = pin.Label,
                Address = pin.Address
            };

            return pinModelDb;
        }


        public static Pin PinViewModelToPin(this PinViewModel pinViewModel)
        {
            Pin pin = new Pin
            {
                Label = pinViewModel.Label,
                Position = new Position(pinViewModel.Latitude, pinViewModel.Longitude),
                IsVisible = pinViewModel.IsEnabled,
                Address = pinViewModel.Address
            };

            return pin;
        }


        public static PinViewModel PinToPinViewModel(this Pin pin)
        {
            PinViewModel pinViewModel = new PinViewModel
            {
                Label = pin.Label,
                Latitude = pin.Position.Latitude,
                Longitude = pin.Position.Longitude,
                IsEnabled = pin.IsVisible,
                Address = pin.Address
            };

            return pinViewModel;
        }


        public static PinModelDb PinViewModelToPinModelDb(this PinViewModel pinViewModel)
        {
            PinModelDb pinModelDb = new PinModelDb
            {
                Id = pinViewModel.Id,
                Label = pinViewModel.Label,
                Latitude = pinViewModel.Latitude,
                Longitude = pinViewModel.Longitude,
                Address = pinViewModel.Address,
                Description = pinViewModel.Description,
                IsEnable = pinViewModel.IsEnabled
            };

            return pinModelDb;
        }


        public static PinViewModel PinModelDbToPinViewModel(this PinModelDb pinModelDb)
        {
            PinViewModel pinViewModel = new PinViewModel
            {
                Id = pinModelDb.Id,
                Latitude = pinModelDb.Latitude,
                Longitude = pinModelDb.Longitude,
                Label = pinModelDb.Label,
                Address = pinModelDb.Address,
                Description = pinModelDb.Description,
                IsEnabled = pinModelDb.IsEnable
            };

            return pinViewModel;
        }


        public static Pin PinModelDbToPin(this PinModelDb pinModelDb)
        {
            Pin pin = new Pin
            {
                Label = pinModelDb.Label,
                Position = new Position(pinModelDb.Latitude, pinModelDb.Longitude),
                Address = pinModelDb.Address,
                IsVisible = pinModelDb.IsEnable
            };

            return pin;
        }
    }
}
