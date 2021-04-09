using GpsNote.Models;
using GpsNote.Services.PinService;
using GpsNote.Services.RepositoryService;
using GpsNote.Services.SettingsService;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Extensions;

namespace GpsNote.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        #region -- Private fields -- 

        private IPageDialogService _dialogService;
        private IPinService        _pinService;
        private ISettingsService _settings;
        //private IRepositoryService _repository;

        #endregion


        #region -- Constructors --

        public MapViewModel(INavigationService navigationService, IPageDialogService dialogService, ISettingsService settings, IRepositoryService repository, IPinService pinService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pinService = pinService;
            _settings = settings;
            //_repository = repository;

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


        private DelegateCommand<Object> mapTapCommand;
        public DelegateCommand<Object> MapTapCommand => mapTapCommand ?? (new DelegateCommand<Object>(OnClickMapAsync));

        private DelegateCommand<Object> cameraIdLedCommand;
        public DelegateCommand<Object> CameraIdLedCommand => cameraIdLedCommand ?? (new DelegateCommand<Object>(OnCameraLed));

        #endregion


        #region -- Private helpers -- 

        private async void OnClickMapAsync(Object _position)
        {
            //Position position = (Position)_position;

            //Pin pin = await _pinService.GetNewPinAsync(position);
            //List<Pin> addedPin = new List<Pin>();
            //addedPin.Add(pin);

            //PinModel pinModel = pin.PinToPinModel();

            //try
            //{
            //    _pinService.SavePinModelToDatabase(pinModel);
            //}
            //catch (Exception ex)
            //{
            //    await _dialogService.DisplayAlertAsync(title: "Error",
            //                                     message: ex.Message,
            //                                     cancelButton: "Close");
            //    return;
            //}

            //Pins = addedPin;
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
            try
            {
                Pins = _pinService.GetUserPins();
            }
            catch (Exception ex)
            {
                _dialogService.DisplayAlertAsync(title: "Error",
                                                 message: ex.Message,
                                                 cancelButton: "Close");
                return;
            }
        }

        #endregion
    }
}