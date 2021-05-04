using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms.GoogleMaps;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using GpsNote.Extensions;
using GpsNote.Models;
using GpsNote.Services.Authorization;
using GpsNote.Services.PinService;
using GpsNote.ViewModels.ExtentedViewModels;
using GpsNote.Services.Localization;
using GpsNote.Helpers;


namespace GpsNote.ViewModels
{
    public class AddEditPinViewModel : ViewModelBase
    {
        private IPinService _pinService;
        private IPageDialogService _dialogService;
        private IAuthorizationService _authorizationService;
        private bool editMode = false;
        private PinViewModel editPinViewModel = null;


        public AddEditPinViewModel(INavigationService navigationService,
                                   ILocalizationService localizationService,
                                   IPageDialogService dialogService,
                                   IPinService pinService,
                                   IAuthorizationService authorizeService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
            _dialogService = dialogService;
            _authorizationService = authorizeService;
        }


        #region -- Publics --

        private string labelPinText;
        public string LabelPinText
        {
            get => labelPinText;
            set => SetProperty(ref labelPinText, value);
        }

        private string latitudePinText;
        public string LatitudePinText
        {
            get => latitudePinText;
            set => SetProperty(ref latitudePinText, value);
        }

        private string longitudePinText;
        public string LongitudePinText
        {
            get => longitudePinText;
            set => SetProperty(ref longitudePinText, value);
        }

        private string descriptionText;
        public string DescriptionText
        {
            get => descriptionText;
            set => SetProperty(ref descriptionText, value);
        }

        private List<Pin> pins;
        public List<Pin> Pins
        {
            get => pins;
            set => SetProperty(ref pins, value);
        }

        private string labelPinError;
        public string LabelPinError
        {
            get => labelPinError;
            set => SetProperty(ref labelPinError, value);
        }

        private string longtitudePinError;
        public string LongtitudePinError
        {
            get => longtitudePinError;
            set => SetProperty(ref longtitudePinError, value);
        }

        private string latitudePinError;
        public string LatitudePinError
        {
            get => latitudePinError;
            set => SetProperty(ref latitudePinError, value);
        }

        private bool tapLabelImage;
        public bool TapLabelImage
        {
            get => tapLabelImage;
            set => SetProperty(ref tapLabelImage, value);
        }

        private bool tapDescriptionImage;
        public bool TapDescriptionImage
        {
            get => tapDescriptionImage;
            set => SetProperty(ref tapDescriptionImage, value);
        }

        private bool tapLongtitudeImage;
        public bool TapLongtitudeImage
        {
            get => tapLongtitudeImage;
            set => SetProperty(ref tapLongtitudeImage, value);
        }

        private bool tapLatitudeImage;
        public bool TapLatitudeImage
        {
            get => tapLatitudeImage; 
            set => SetProperty(ref tapLatitudeImage, value);
        }


        private CameraUpdate initialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));
        public CameraUpdate InitialCameraUpdate
        {
            get => initialCameraUpdate;
            set => SetProperty(ref initialCameraUpdate, value);
        }

        private DelegateCommand<Object> mapTapCommand;
        public DelegateCommand<Object> MapTapCommand => mapTapCommand ?? new DelegateCommand<Object>(OnMapClickAsync);

        private DelegateCommand saveTapCommand;
        public DelegateCommand SaveTapCommand => saveTapCommand ?? new DelegateCommand(OnSavePinAsync);

        private DelegateCommand backPressedCommand;
        public DelegateCommand BackPressedCommand => backPressedCommand ?? new DelegateCommand(OnBackPressed);

        #endregion


        #region -- Override --

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(PinViewModel), out PinViewModel pinViewModel))
            {
                LabelPinText = pinViewModel.Label;
                LatitudePinText = pinViewModel.Latitude.ToString();
                LongitudePinText = pinViewModel.Longtitude.ToString();
                DescriptionText = pinViewModel.Description;

                Pin pin = pinViewModel.ToPin();

                Pins = new List<Pin>
                {
                    pin
                };
                
                InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                    new CameraPosition(pin.Position, Constants.ZOOM));

                editMode = true;
                editPinViewModel = pinViewModel;
                Title = Resource["EditPinTitle"];
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if(args.PropertyName == nameof(TapLabelImage))
            {
                LabelPinText = string.Empty;
            }
            else if(args.PropertyName == nameof(TapDescriptionImage))
            {
                DescriptionText = string.Empty;
            }
            else if(args.PropertyName == nameof(TapLongtitudeImage))
            {
                LongitudePinText = string.Empty;
            }
            else if(args.PropertyName == nameof(TapLatitudeImage))
            {
                LatitudePinText = string.Empty;
            }
        }

        #endregion


        #region -- Private helpers --

        private async void OnMapClickAsync(Object _position)
        {
            Position position = (Position)_position;
            Pin pin = await _pinService.GetNewPinFromPositionAsync(position);

            string latitude = DoubleOutFormat.Format(pin.Position.Latitude); 
            string longtitude = DoubleOutFormat.Format(pin.Position.Longitude);

            LabelPinText = pin.Label;
            LatitudePinText = latitude;
            LongitudePinText = longtitude;

            if (editMode)
                pin.IsVisible = Pins[0].IsVisible;

            Pins = new List<Pin>
            {
                pin
            };

            LabelPinError = string.Empty;
            LongtitudePinError = string.Empty;
            LatitudePinError = string.Empty;
        }

        private async void OnSavePinAsync()
        {
            if (!string.IsNullOrWhiteSpace(LabelPinText) &&
                !string.IsNullOrWhiteSpace(LatitudePinText) &&
                !string.IsNullOrWhiteSpace(LongitudePinText))
            {
                PinModel pinModel;

                if (editMode)
                {
                    pinModel = await UpdateExistPinAsync();
                }
                else
                {
                    pinModel = await SaveNewPinAsync(Pins[0]);
                }
                
                if(pinModel != null)
                {
                    NavigationParameters parameters = new NavigationParameters();

                    if (editMode)
                    {
                        parameters.Add(Constants.EDIT_PIN, pinModel);
                    }
                    else
                    {
                        parameters.Add(Constants.NEW_PIN, pinModel);
                    }

                    await NavigationService.GoBackAsync(parameters);
                }
                else
                {
                    await _dialogService.DisplayAlertAsync(Resource["ErrorText"],
                                                     Resource["PinSaveError"],
                                                     Resource["CancelText"]);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(LabelPinText))
                {
                    LabelPinError = Resource["LabelPinError"];
                }
                if (string.IsNullOrWhiteSpace(LatitudePinText))
                {
                    LatitudePinError = Resource["LatitudeError"];
                }
                if (string.IsNullOrWhiteSpace(LongitudePinText))
                {
                    LongtitudePinError = Resource["LongtitudeError"];
                }
            }
        }

        private async System.Threading.Tasks.Task<PinModel> UpdateExistPinAsync()
        {
            PinModel pinModel = await _pinService.FindPinModelAsync(p => p.Id == editPinViewModel.Id);

            if (pinModel != null)
            {
                if(double.TryParse(LongitudePinText, out double longtitudeValue) && double.TryParse(LatitudePinText, out  double latitudeValue))
                {
                    pinModel.Label = LabelPinText;
                    pinModel.Description = DescriptionText;
                    pinModel.Owner = _authorizationService.GetCurrentUserID();
                    pinModel.IsEnable = editPinViewModel.IsEnabled;
                    pinModel.Latitude = latitudeValue;
                    pinModel.Longitude = longtitudeValue;
                    int rows = await _pinService.UpdatePinModelAsync(pinModel);

                    if (rows <= 0)
                    {
                        pinModel = null;
                    }
                } 
            }

            return pinModel;
        }

        private async System.Threading.Tasks.Task<PinModel> SaveNewPinAsync(Pin pin) 
        {
            pin.Label = LabelPinText;

            PinModel pinModel = pin.ToPinModel();
            pinModel.Description = DescriptionText;
            pinModel.Owner = _authorizationService.GetCurrentUserID();
            pinModel.IsEnable = true;

            int rows = await _pinService.SavePinModelToDatabaseAsync(pinModel);

            if(rows == 0)
            {
                pinModel = null;
            }

            return pinModel;
        }

        private void OnBackPressed()
        {
            NavigationService.GoBackAsync();
        }

        #endregion
    }
}