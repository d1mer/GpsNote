using GpsNote.Controls;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        IPageDialogService _dialogService;
        public MapViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
        }


        #region -- Publics -- 
        MapType mapType;
        public MapType MapType
        {
            get => mapType;
            set => SetProperty(ref mapType, value);
        }

        public DelegateCommand<Position> TapCommand => new DelegateCommand<Position>(EventTap);
        //public DelegateCommand TapCommand => new DelegateCommand(EventTap);

        #endregion

        private async void EventTap(Position position)
        {
            await _dialogService.DisplayAlertAsync("Position", $"Latitude - {position.Latitude}\nLongitude - {position.Longitude}", "Cancel");
            //await _dialogService.DisplayAlertAsync("Position", $"RRRRR", "Cancel");
        }
    }
}
