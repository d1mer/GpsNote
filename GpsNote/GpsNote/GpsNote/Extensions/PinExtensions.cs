using GpsNote.Models;
using GpsNote.ViewModels.ExtentedViewModels;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Extensions
{
    public static class PinExtensions
    {
        public static PinModel ToPinModel(this Pin pin)
        {
            PinModel pinModel = new PinModel
            {
                Latitude = pin.Position.Latitude,
                Longitude = pin.Position.Longitude,
                Label = pin.Label,
                Address = pin.Address
            };

            return pinModel;
        }

        public static PinModel ToPinModel(this PinViewModel pinViewModel)
        {
            PinModel pinModel = new PinModel
            {
                Id = pinViewModel.Id,
                Label = pinViewModel.Label,
                Latitude = pinViewModel.Latitude,
                Longitude = pinViewModel.Longitude,
                Address = pinViewModel.Address,
                Description = pinViewModel.Description,
                IsEnable = pinViewModel.IsEnabled
            };

            return pinModel;
        }

        public static Pin ToPin(this PinViewModel pinViewModel)
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

        public static Pin ToPin(this PinModel pinModel)
        {
            Pin pin = new Pin
            {
                Label = pinModel.Label,
                Position = new Position(pinModel.Latitude, pinModel.Longitude),
                Address = pinModel.Address,
                IsVisible = pinModel.IsEnable
            };

            return pin;
        }

        public static PinViewModel ToPinViewModel(this Pin pin)
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

        public static PinViewModel ToPinViewModel(this PinModel pinModel)
        {
            PinViewModel pinViewModel = new PinViewModel
            {
                Id = pinModel.Id,
                Latitude = pinModel.Latitude,
                Longitude = pinModel.Longitude,
                Label = pinModel.Label,
                Address = pinModel.Address,
                Description = pinModel.Description,
                IsEnabled = pinModel.IsEnable
            };

            return pinViewModel;
        }       
    }
}