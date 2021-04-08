using GpsNote.Models;
using GpsNote.Services.Repository;
using GpsNote.Services.Settings;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        #region -- Private fields -- 

        private IPageDialogService _dialogService;
        private ISettingsService _settings;
        private IRepositoryService _repository;

        #endregion


        #region -- Constructors --

        public MapViewModel(INavigationService navigationService, IPageDialogService dialogService, ISettingsService settings, IRepositoryService repository) : base(navigationService)
        {
            _dialogService = dialogService;
            _settings = settings;
            _repository = repository;

            Title = "Map";

            InitialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));

            //_repository.DeleteAllAsync<PinModel>();

            if (_settings.LastZoom != 0.0)
            {
                Position position = new Position(_settings.LastLatitude, _settings.LastLongitude);

                InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                    new CameraPosition(position,
                                       _settings.LastZoom,
                                       _settings.LastBearing,
                                       _settings.LastTilt));
            }

            Task.Run(() => GetPinsFromDatabase());
        }

        #endregion


        #region -- Publics -- 

        private CameraUpdate initialCameraUpdate;
        public CameraUpdate InitialCameraUpdate
        {
            get => initialCameraUpdate;
            set => SetProperty(ref initialCameraUpdate, value);
        }

        private MapType mapType;
        public MapType MapType
        {
            get => mapType;
            set => SetProperty(ref mapType, value);
        }

        private List<Pin> pins = new List<Pin>();
        public List<Pin> Pins
        {
            get => pins;
            set => SetProperty(ref pins, value);
        }


        public DelegateCommand<Object> MapTapCommand => new DelegateCommand<Object>(OnClickMapAsync);
        public DelegateCommand<Object> CameraIdLedCommand => new DelegateCommand<Object>(OnCameraLed);

        #endregion


        #region -- Private helpers -- 

        private async void OnClickMapAsync(Object _position)
        {
            Position position = (Position)_position;

            Geocoder geocoder = new Geocoder();
            IEnumerable<string> address = await geocoder.GetAddressesForPositionAsync(position);

            Pin pin = new Pin
            {
                Position = position,
                Address = address != null ? address.FirstOrDefault() : string.Empty,
                Label = address != null ?
                        address.FirstOrDefault().Substring(0, address.FirstOrDefault().IndexOf(",") != -1 ?
                                                              address.FirstOrDefault().IndexOf(",") :
                                                              address.FirstOrDefault().Length - 1) :
                        "New pin"
            };
            List<Pin> addedPin = new List<Pin>();
            addedPin.Add(pin);

            PinModel pinModel = new PinModel
            {
                Latitude = pin.Position.Latitude,
                Longitude = pin.Position.Longitude,
                Label = pin.Label,
                Address = pin.Address,
                Owner = _settings.IdCurrentUser
            };

            try
            {
                await _repository.InsertAsync<PinModel>(pinModel);
            }
            catch (Exception ex)
            {
                await _dialogService.DisplayAlertAsync(title: "Error",
                                                 message: ex.Message,
                                                 cancelButton: "Close");
                return;
            }

            Pins = addedPin;
        }


        private async void OnCameraLed(Object _cameraPosition)
        {
            CameraPosition cameraPosition = _cameraPosition as CameraPosition;
            await Task.Run(() => RecordCurrentCameraPosition(cameraPosition));
        }

        private void RecordCurrentCameraPosition(CameraPosition cameraPosition)
        {
            _settings.LastLatitude = cameraPosition.Target.Latitude;
            _settings.LastLongitude = cameraPosition.Target.Longitude;
            _settings.LastZoom = cameraPosition.Zoom;
            _settings.LastBearing = cameraPosition.Bearing;
            _settings.LastTilt = cameraPosition.Tilt;
        }


        private void GetPinsFromDatabase()
        {
            List<PinModel> pinModels;

            try
            {
                pinModels = _repository.GetAllAsync<PinModel>(p => p.Owner == _settings.IdCurrentUser).Result;
            }
            catch (Exception ex)
            {
                _dialogService.DisplayAlertAsync(title: "Error",
                                                 message: ex.Message,
                                                 cancelButton: "Close");
                return;
            }

            if (pinModels == null || pinModels.Count == 0)
                return;

            List<Pin> pinsList = new List<Pin>();

            foreach (PinModel pinModel in pinModels)
            {
                Pin pin = new Pin();
                pin.Position = new Position(pinModel.Latitude, pinModel.Longitude);
                pin.Label = pinModel.Label;
                pin.Address = pinModel.Address;
                pinsList.Add(pin);
            }

            Pins = pinsList;
        }

        #endregion
    }
}