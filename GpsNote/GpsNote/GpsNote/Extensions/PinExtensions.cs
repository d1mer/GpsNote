using System;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Helpers;
using GpsNote.Models;
using GpsNote.ViewModels.ExtentedViewModels;

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
                Longitude = pinViewModel.Longtitude,
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
                Position = new Position(pinViewModel.Latitude, pinViewModel.Longtitude),
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
            double latitude = GetDoubleTruncated(pin.Position.Latitude);
            double longtitude = GetDoubleTruncated(pin.Position.Longitude);

            PinViewModel pinViewModel = new PinViewModel
            {
                Label = pin.Label,
                Latitude = latitude,
                Longtitude = longtitude,
                IsEnabled = pin.IsVisible,
                Address = pin.Address
            };

            return pinViewModel;
        }

        public static PinViewModel ToPinViewModel(this PinModel pinModel)
        {
            double latitude = GetDoubleTruncated(pinModel.Latitude);
            double longtitude = GetDoubleTruncated(pinModel.Longitude);

            PinViewModel pinViewModel = new PinViewModel
            {
                Id = pinModel.Id,
                Latitude = latitude,
                Longtitude = longtitude,
                Label = pinModel.Label,
                Address = pinModel.Address,
                Description = pinModel.Description,
                IsEnabled = pinModel.IsEnable
            };

            return pinViewModel;
        }       

        private static double GetDoubleTruncated(double digit)
        {
            string str = DoubleOutFormat.Format(digit);
            return Convert.ToDouble(str);
        }
    }
}