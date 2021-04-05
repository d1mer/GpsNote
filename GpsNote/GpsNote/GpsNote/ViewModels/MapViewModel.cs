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
        private ISettingsService _settings;
        private IRepository _repository;

        #endregion


        #region -- Constructors --

        public MapViewModel(INavigationService navigationService, IPageDialogService dialogService, ISettingsService settings, IRepository repository) : base(navigationService)
        {
            _dialogService = dialogService;
            _settings = settings;
            _repository = repository;

            Title = "Map";
          
            if (_settings.LastZoom != 0.0)
            {
                Position position = new Position(_settings.LastLatitude, _settings.LastLongitude);
               
                InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                    new CameraPosition(position, 
                                       _settings.LastZoom, 
                                       _settings.LastBearing, 
                                       _settings.LastTilt));
            }
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


        public DelegateCommand<Object> MapTapCommand => new DelegateCommand<Object>(OnClickMap);
        public DelegateCommand<Object> CameraIdLedCommand => new DelegateCommand<Object>(OnCameraLed);

        #endregion



        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            //if(args.PropertyName == nameof(LastPosition) ||
            //    args.PropertyName == nameof(LastZoom)    ||
            //    args.PropertyName == nameof(LastBearing) ||
            //    args.PropertyName == nameof(LastTilt))
            //{
            //    _settings.LastPosition = RecordPosition.WritePositionToString(LastPosition);
            //    _settings.LastZoom = LastZoom;
            //    _settings.LastBearing = LastBearing;
            //    _settings.LastTilt = LastTilt;
            //}

            //if(args.PropertyName == nameof(PinAdded))
            //{
            //    PinModel pinModel = new PinModel
            //    {
            //        Pin = PinAdded,
            //        Owner = _settings.IdCurrentUser
            //    };
            //}
        }

        #endregion

        private async void OnClickMap(Object _position)
        {
            Position position = (Position)_position;
            //await _dialogService.DisplayAlertAsync("Position", $"Latitude - {position.Latitude}\nLongitude - {position.Longitude}", "Cancel");
        }

        private async void OnCameraLed(Object _cameraPosition)
        {
            CameraPosition cameraPosition = _cameraPosition as CameraPosition;
            await Task.Run(() => RecPos(cameraPosition));
        }

        private void RecPos(CameraPosition cameraPosition)
        {
            _settings.LastLatitude  = cameraPosition.Target.Latitude;
            _settings.LastLongitude = cameraPosition.Target.Longitude;
            _settings.LastZoom      = cameraPosition.Zoom;
            _settings.LastBearing   = cameraPosition.Bearing;
            _settings.LastTilt      = cameraPosition.Tilt;
        }
    }
}