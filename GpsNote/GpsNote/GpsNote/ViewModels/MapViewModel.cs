using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Services.MapCameraSettingsService;
using GpsNote.Services.PinService;
using GpsNote.Models;
using GpsNote.Extensions;
using GpsNote.Services.Permissions;
using Rg.Plugins.Popup.Services;
using GpsNote.Views.Clock;
using GpsNote.Services.TimeZone;

namespace GpsNote.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private IPageDialogService _dialogService;
        private IPinService        _pinService;
        private IMapCameraSettingsService _cameraSettingsService;
        private IPermissionsService _permissionsService;
        private ITimeZoneService _timeZoneService;


        public MapViewModel(INavigationService navigationService, 
                            IPageDialogService dialogService, 
                            IPinService pinService, 
                            IMapCameraSettingsService cameraSettingsService,
                            IPermissionsService permissionsService,
                            ITimeZoneService timeZoneService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pinService = pinService;
            _cameraSettingsService = cameraSettingsService;
            _permissionsService = permissionsService;
            _timeZoneService = timeZoneService;

            Title = "Map";

            InitialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));
        }


        #region -- Publics -- 

        private CameraUpdate initialCameraUpdate;
        public CameraUpdate InitialCameraUpdate
        {
            get => initialCameraUpdate;
            set => SetProperty(ref initialCameraUpdate, value);
        }

        private List<Pin> pins;
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

        private bool isVisibleSearcBar = false;
        public bool IsVisibleSearcBar
        {
            get => isVisibleSearcBar;
            set => SetProperty(ref isVisibleSearcBar, value);
        }

        private bool isVisibleSearchButton = true;
        public bool IsVisibleSearchButton
        {
            get => isVisibleSearchButton;
            set => SetProperty(ref isVisibleSearchButton, value);
        }

        private bool myLocationbuttonsVisibility = true;
        public bool MyLocationButtonVisibility
        {
            get => myLocationbuttonsVisibility;
            set => SetProperty(ref myLocationbuttonsVisibility, value);
        }


        private bool isSearchListVisible = false;
        public bool IsSearchListVisible
        {
            get => isSearchListVisible;
            set => SetProperty(ref isSearchListVisible, value);
        }

        private ObservableCollection<Pin> searchResultList;
        public ObservableCollection<Pin> SearchResultList
        {
            get => searchResultList;
            set => SetProperty(ref searchResultList, value);
        }

        private int listRowHeight = 100;
        public int ListRowHeight
        {
            get => listRowHeight;
            set => SetProperty(ref listRowHeight, value);
        }

        private int listHeiqhtRequest;
        public int ListHeiqhtRequest
        {
            get => listHeiqhtRequest;
            set => SetProperty(ref listHeiqhtRequest, value);
        }


        private DelegateCommand<Object> cameraIdLedCommand;
        public DelegateCommand<Object> CameraIdLedCommand => cameraIdLedCommand ?? new DelegateCommand<Object>(OnCameraLed);

        private DelegateCommand searchButtonTapCommand;
        public DelegateCommand SearchButtonTapCommand => searchButtonTapCommand ?? new DelegateCommand(OnSearchButtonTap);

        private DelegateCommand<object> searchTextChangedCommand;
        public DelegateCommand<object> SearchTextChangedCommand => searchTextChangedCommand ?? new DelegateCommand<object>(OnSearchPin);

        private DelegateCommand<object> unfocusedSearchbarCommand;
        public DelegateCommand<object> UnfocusedSearchbarCommand => unfocusedSearchbarCommand ?? new DelegateCommand<object>(OnUnfocusedSearch);

        private DelegateCommand<object> mapTapCommand;
        public DelegateCommand<object> MapTapCommand => mapTapCommand ?? new DelegateCommand<object>(OnMapClick);

        private DelegateCommand<object> listItemTapCommand;
        public DelegateCommand<Object> ListItemTapCommand => listItemTapCommand ?? new DelegateCommand<object>(OnItemTap);

        private DelegateCommand<object> pinClickedCommand;
        public DelegateCommand<object> PinClickedCommand => pinClickedCommand ?? new DelegateCommand<object>(OnPinClickedAsync);

        #endregion


        #region -- Override --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(_cameraSettingsService.GetInitialCameraSettings());

            Task.Run(() => GetPinsFromDatabaseAsync());

            if (!_permissionsService.CheckLocationPermission())
            {
                MyLocationButtonVisibility = false;
            }
            else
            {
                MyLocationButtonVisibility = true;
            }
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Task.Run(() => GetPinsFromDatabaseAsync());

            if (parameters.TryGetValue<Position>(Constants.DISPLAY_PIN, out Position position))
            {
                MovingCameraPosition = position;
                IsMoveCamera = true;
            }
        }

        #endregion


        #region -- Private helpers -- 

        private void OnCameraLed(Object _cameraPosition)
        {
            CameraPosition cameraPosition = _cameraPosition as CameraPosition;
            _cameraSettingsService.SaveCurrentCameraPositionAsync(cameraPosition);
        }

        private async void GetPinsFromDatabaseAsync()
        {
            List<PinModel> userPins = await _pinService.GetUsersPinsAsync();

            if (userPins != null)
            {
                List<Pin> pins = new List<Pin>();
                Pin pin;

                foreach (PinModel pinModel in userPins)
                {
                    pin = pinModel.ToPin();
                    pins.Add(pin);
                }

                Pins = pins;
            }
        }

        private void OnSearchButtonTap()
        {
            IsVisibleSearcBar = true;
            IsVisibleSearchButton = false;
            MyLocationButtonVisibility = false;
        }

        private void OnSearchPin(object obj)
        {
            string newText = obj as string;

            if (!string.IsNullOrWhiteSpace(newText))
            {
                var list = Pins.Where(p => p.Label.Contains(newText, StringComparison.OrdinalIgnoreCase)).ToList();
                SearchResultList = new ObservableCollection<Pin>(list);
                ListHeiqhtRequest = ListRowHeight * list.Count;
                IsSearchListVisible = true;
            }
            else
            {
                IsSearchListVisible = false;
            }
        }

        private void OnItemTap(object obj)
        {
            Pin pin = obj as Pin;

            if (pin != null)
            {
                MovingCameraPosition = pin.Position;
                IsMoveCamera = true;
            }
        }

        private void OnUnfocusedSearch(object obj)
        {
            IsVisibleSearcBar = false;
            IsVisibleSearchButton = true;
            MyLocationButtonVisibility = _permissionsService.CheckLocationPermission();
            IsSearchListVisible = false;
        }

        private void OnMapClick(object obj)
        {
            IsVisibleSearcBar = false;
            IsVisibleSearchButton = true;
            MyLocationButtonVisibility = _permissionsService.CheckLocationPermission();
        }

        private async void OnPinClickedAsync(object obj)
        {
            Pin pin = obj as Pin;
            await PopupNavigation.Instance.PushAsync(new ClockPopupPage());
            //if (pin != null)
            //{
            //    TimeZoneResponse timeZoneResponse = await _timeZoneService.GetTimeZoneAsync(pin.Position);
                
            //    if(timeZoneResponse.Status == "OK")
            //    {
            //        await PopupNavigation.Instance.PushAsync(new ClockPopupPage());
            //    }
            //    else
            //    {
            //        await _dialogService.DisplayAlertAsync("Error get TimeZone", timeZoneResponse.Status, "Cancel");
            //    }
            //}
        }

        #endregion
    }
}