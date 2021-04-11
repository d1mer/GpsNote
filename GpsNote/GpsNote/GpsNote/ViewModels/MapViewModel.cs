using GpsNote.Services.PinService;
using GpsNote.Services.SettingsService;
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
        private IPinService        _pinService;
        private ISettingsService _settings;

        #endregion


        #region -- Constructors --

        public MapViewModel(INavigationService navigationService, IPageDialogService dialogService, ISettingsService settings, IPinService pinService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pinService = pinService;
            _settings = settings;

            Title = "Map";

            InitialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));

            if (_settings.LastZoom != 0.0)
            {
                Position position = new Position(_settings.LastLatitude, _settings.LastLongitude);

                InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                    new CameraPosition(position,
                                       _settings.LastZoom,
                                       _settings.LastBearing,
                                       _settings.LastTilt));
            }

            Task.Run(() => GetPinsFromDatabaseAsync());
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


        private bool isMoveCamera;
        public bool IsMoveCamera
        {
            get => isMoveCamera;
            set => SetProperty(ref isMoveCamera, value);
        }

        private Position movingCameraPosition;
        public Position MovingCameraPosition
        {
            get => movingCameraPosition;
            set => SetProperty(ref movingCameraPosition, value);
        }

        private DelegateCommand<Object> cameraIdLedCommand;
        public DelegateCommand<Object> CameraIdLedCommand => cameraIdLedCommand ?? (new DelegateCommand<Object>(OnCameraLed));

        #endregion


        #region -- Override --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Task.Run(() => GetPinsFromDatabaseAsync());

            if (_pinService.IsDisplayConcretePin)
            {
                MovingCameraPosition = parameters.GetValue<Position>("displayPin");
                IsMoveCamera = true;
                _pinService.IsDisplayConcretePin = false;
            }
        }

        #endregion


        #region -- Private helpers -- 

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


        private async void GetPinsFromDatabaseAsync()
        {
            List<Pin> pinList;
            try
            {
                pinList = await _pinService.GetUserPinModelDbToPinsFromDatabaseAsync();
            }
            catch (Exception ex)
            {
                await _dialogService.DisplayAlertAsync(title: "Error",
                                                 message: ex.Message,
                                                 cancelButton: "Close");
                return;
            }

            Pins = pinList.Where(p => p.IsVisible == true).ToList();
        }

        #endregion
    }
}