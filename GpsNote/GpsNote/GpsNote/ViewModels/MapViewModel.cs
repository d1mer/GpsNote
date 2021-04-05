using GpsNote.Controls;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Services.Settings;
using GpsNote.Helpers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.ComponentModel;
using GpsNote.Services.Repository;
using GpsNote.Models;

namespace GpsNote.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        #region -- Private fields -- 

        private IPageDialogService _dialogService;
        private ISettings _settings;
        private IRepository _repository;
        private bool _flag = true;
        #endregion

       

        public MapViewModel(INavigationService navigationService, IPageDialogService dialogService, ISettings settings, IRepository repository) : base(navigationService)
        {
            _dialogService = dialogService;
            _settings = settings;
            _repository = repository;

            
            if (!string.IsNullOrEmpty(_settings.LastPosition))
            {
                IsRequiredCameraChanging = true;
                _flag = false;
                LastPosition = RecordPosition.ReadPositionFromString(_settings.LastPosition);
                LastZoom = _settings.LastZoom;
                LastBearing = _settings.LastBearing;
                LastTilt = _settings.LastTilt;
                _flag = true;
            }

           Pins = _repository.GetPinsAsync(_settings.LoggedUser).Result;
        }

        
        
        #region -- Publics -- 

        private MapType mapType;
        public MapType MapType
        {
            get => mapType;
            set => SetProperty(ref mapType, value);
        }

        private Position lastPosition;
        public Position LastPosition
        {
            get => lastPosition;
            set => SetProperty(ref lastPosition, value);
        }

        private double lastZoom;
        public double LastZoom
        {
            get => lastZoom;
            set => SetProperty(ref lastZoom, value);
        }

        private double lastBearing;
        public double LastBearing
        {
            get => lastBearing;
            set => SetProperty(ref lastBearing, value);
        }

        private double lastTilt;
        public double LastTilt
        {
            get => lastTilt;
            set => SetProperty(ref lastTilt, value);
        }

        private bool isRequiredCameraChanging;
        public bool IsRequiredCameraChanging
        {
            get => isRequiredCameraChanging;
            set => SetProperty(ref isRequiredCameraChanging, value);
        }

        private List<Pin> pins;
        public List<Pin> Pins
        {
            get => pins;
            set => SetProperty(ref pins, value);
        }

        private Pin pinAdded;
        public Pin PinAdded
        {
            get => pinAdded;
            set => SetProperty(ref pinAdded, value);
        }

        public DelegateCommand<Object> MapTapCommand => new DelegateCommand<Object>(OnClickMap);
        public DelegateCommand<Object> CameraIdLedCommand => new DelegateCommand<Object>(OnCameraLed);

        #endregion



        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (!_flag)
            {
                return;
            }

            base.OnPropertyChanged(args);

            if(args.PropertyName == nameof(LastPosition) ||
                args.PropertyName == nameof(LastZoom)    ||
                args.PropertyName == nameof(LastBearing) ||
                args.PropertyName == nameof(LastTilt))
            {
                _settings.LastPosition = RecordPosition.WritePositionToString(LastPosition);
                _settings.LastZoom = LastZoom;
                _settings.LastBearing = LastBearing;
                _settings.LastTilt = LastTilt;
            }

            if(args.PropertyName == nameof(PinAdded))
            {
                PinModel pinModel = new PinModel
                {
                    Pin = PinAdded,
                    Owner = _settings.LoggedUser
                };
            }
        }

        #endregion

        private async void OnClickMap(Object _position)
        {
            Position position = (Position)_position;
            await _dialogService.DisplayAlertAsync("Position", $"Latitude - {position.Latitude}\nLongitude - {position.Longitude}", "Cancel");
        }

        private async void OnCameraLed(Object _cameraPosition)
        {
            CameraPosition cameraPosition = _cameraPosition as CameraPosition;
            await Task.Run(() => RecPos(cameraPosition));
        }

        private void RecPos(CameraPosition cameraPosition)
        {
            _settings.LastPosition = RecordPosition.WritePositionToString(new Position(cameraPosition.Target.Latitude, cameraPosition.Target.Longitude));
        }
    }
}
