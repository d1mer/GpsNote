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

namespace GpsNote.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        #region -- Private fields -- 

        IPageDialogService _dialogService;
        ISettings _settings;

        #endregion
        public MapViewModel(INavigationService navigationService, IPageDialogService dialogService, ISettings settings) : base(navigationService)
        {
            _dialogService = dialogService;
            _settings = settings;

            Xamarin.Forms.Element.
            if (!string.IsNullOrEmpty(_settings.LastPosition))
            {
                Position position = RecordPosition.ReadPositionFromString(_settings.LastPosition);
                
            }
        }

        
        #region -- Publics -- 
        MapType mapType;
        public MapType MapType
        {
            get => mapType;
            set => SetProperty(ref mapType, value);
        }

        public DelegateCommand<Object> MapTapCommand => new DelegateCommand<Object>(OnClickMap);
        public DelegateCommand<Object> CameraIdLedCommand => new DelegateCommand<Object>(OnCameraLed);

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
            _settings.LastPosition = RecordPosition.WritePositionToString(new Position(cameraPosition.Target.Latitude, cameraPosition.Target.Longitude));
        }
    }
}
