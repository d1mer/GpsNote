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

namespace GpsNote.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        #region -- Private fields -- 

        private IPageDialogService _dialogService;
        private ISettings _settings;
        private bool _flag = true;
        #endregion

       

        public MapViewModel(INavigationService navigationService, IPageDialogService dialogService, ISettings settings) : base(navigationService)
        {
            _dialogService = dialogService;
            _settings = settings;

            
            if (!string.IsNullOrEmpty(_settings.LastPosition))
            {
                IsRequiredCameraChanging = true;
                _flag = false;
                LastPosition = RecordPosition.ReadPositionFromString(_settings.LastPosition);
                LastZoom = _settings.LastZoom;
                LastBearing = _settings.LastBearing;
                LastTilt = _settings.LastTilt;
                _flag = true;
                //IsRequiredCameraChanging = true;
            }
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
