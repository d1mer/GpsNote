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
            MapType = MapType.Satellite;
            _dialogService = dialogService;
        }

        MapType mapType;
        public MapType MapType
        {
            get => mapType;
            set => SetProperty(ref mapType, value);
        }

        public DelegateCommand TapCommand => new DelegateCommand(EventTap);

        private async void EventTap()
        {
           await _dialogService.DisplayAlertAsync("SUCCESS",
                                                       "Account created!!!",
                                                       "Cancel");
        }

        //private CustomMap map = new CustomMap();
        //public Map Map
        //{
        //    get => map;
        //    set => SetProperty(ref map, value);
        //}
    }
}
