using GpsNote.Services.MapCameraSettingsService;
using GpsNote.Services.PinService;
using GpsNote.Services.SettingsService;
using GpsNote.ViewModels.ExtentedViewModels;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ICameraSettingsService _cameraSettingsService;

        #endregion


        #region -- Constructors --

        public MapViewModel(INavigationService navigationService, IPageDialogService dialogService, IPinService pinService, ICameraSettingsService cameraSettingsService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pinService = pinService;
            _cameraSettingsService = cameraSettingsService;

            Title = "Map";

            InitialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));
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

        private bool buttonsVisibility = true;
        public bool ButtonsVisibility
        {
            get => buttonsVisibility;
            set => SetProperty(ref buttonsVisibility, value);
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


        private DelegateCommand<Object> cameraIdLedCommand;
        public DelegateCommand<Object> CameraIdLedCommand => cameraIdLedCommand ?? (new DelegateCommand<Object>(OnCameraLed));

        private DelegateCommand searchButtonTapCommand;
        public DelegateCommand SearchButtonTapCommand => searchButtonTapCommand ?? (new DelegateCommand(OnSearchButtonTap));

        private DelegateCommand<object> searchTextChangedCommand;
        public DelegateCommand<object> SearchTextChangedCommand => searchTextChangedCommand ?? (new DelegateCommand<object>(SearchPin));


        private DelegateCommand<object> unfocusedSearchbarCommand;
        public DelegateCommand<object> UnfocusedSearchbarCommand => unfocusedSearchbarCommand ?? (new DelegateCommand<object>(UnfocusedSearch));

        private DelegateCommand<object> mapTapCommand;
        public DelegateCommand<object> MapTapCommand => mapTapCommand ?? (new DelegateCommand<object>(OnMapClick));


        #endregion


        #region -- Override --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(_cameraSettingsService.GetInitialCameraSettings());

            Task.Run(() => GetPinsFromDatabaseAsync());
        }

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

        private void OnCameraLed(Object _cameraPosition)
        {
            CameraPosition cameraPosition = _cameraPosition as CameraPosition;
            _cameraSettingsService.RecordCurrentCameraPositionAsync(cameraPosition);
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


        private void OnSearchButtonTap()
        {
            IsVisibleSearcBar = true;
            IsVisibleSearchButton = false;
            ButtonsVisibility = false;
        }


        private void SearchPin(object obj)
        {
            string newText = obj as string;

            if (string.IsNullOrWhiteSpace(newText))
            {
                IsSearchListVisible = false;
                return;
            }

            var list = Pins.Where(p => p.Label.Contains(newText, StringComparison.OrdinalIgnoreCase)).ToList();
            SearchResultList = new ObservableCollection<Pin>(list);
            IsSearchListVisible = true;
        }


        private void UnfocusedSearch(object obj)
        {
            IsVisibleSearcBar = false;
            IsVisibleSearchButton = true;
            ButtonsVisibility = true;
            IsSearchListVisible = false;
        }


        private void OnMapClick(object obj)
        {
            IsVisibleSearcBar = false;
            IsVisibleSearchButton = true;
            ButtonsVisibility = true;
        }

        #endregion
    }
}