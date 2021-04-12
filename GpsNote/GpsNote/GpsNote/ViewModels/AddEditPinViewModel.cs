using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using GpsNote.Extensions;
using GpsNote.Models;
using GpsNote.Services.AuthorizeService;
using GpsNote.Services.PinService;
using GpsNote.ViewModels.ExtentedViewModels;
using GpsNote.Constants;



namespace GpsNote.ViewModels
{
    public class AddEditPinViewModel : ViewModelBase
    {

        #region -- Private fields --

        private IPinService _pinService;
        private IPageDialogService _dialogService;
        private IAuthorizeService _authorizeService;
        private bool editMode = false;
        private PinViewModel editPinViewModel = null;

        #endregion


        #region -- Constructor --

        public AddEditPinViewModel(INavigationService navigationService,
                                   IPageDialogService dialogService,
                                   IPinService pinService,
                                   IAuthorizeService authorizeService) : base(navigationService)
        {
            _pinService = pinService;
            _dialogService = dialogService;
            _authorizeService = authorizeService;

            Title = "AddEditPin";
        }

        #endregion


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

        private string editorText;
        public string EditorText
        {
            get => editorText;
            set => SetProperty(ref editorText, value);
        }


        private List<Pin> pins;
        public List<Pin> Pins
        {
            get => pins;
            set => SetProperty(ref pins, value);
        }


        private CameraUpdate initialCameraUpdate = CameraUpdateFactory.NewPosition(new Position(0, 0));
        public CameraUpdate InitialCameraUpdate
        {
            get => initialCameraUpdate;
            set => SetProperty(ref initialCameraUpdate, value);
        }


        private DelegateCommand<Object> mapTapCommand;
        public DelegateCommand<Object> MapTapCommand => mapTapCommand ?? (new DelegateCommand<Object>(OnMapClickAsync));

        private DelegateCommand saveTapCommand;
        public DelegateCommand SaveTapCommand => saveTapCommand ?? (new DelegateCommand(OnSavePinAsync));

        #endregion


        #region -- Override --

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(PinViewModel), out PinViewModel pinViewModel))
            {
                LabelPinText = pinViewModel.Label;
                LatitudePinText = pinViewModel.Latitude.ToString();
                LongitudePinText = pinViewModel.Longitude.ToString();
                EditorText = pinViewModel.Description;

                Pin pin = pinViewModel.PinViewModelToPin();

                Pins = new List<Pin>
                {
                    pin
                };
                // TODO: replace zoom value with constant
                InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                    new CameraPosition(pin.Position, 12));

                editMode = true;
                editPinViewModel = pinViewModel;
            }
        }

        #endregion


        #region -- Private helpers --

        private async void OnMapClickAsync(Object _position)
        {
            Position position = (Position)_position;
            Pin pin = await _pinService.GetNewPinFromPositionAsync(position);

            LabelPinText = pin.Label;
            LatitudePinText = pin.Position.Latitude.ToString();
            LongitudePinText = pin.Position.Longitude.ToString();

            if (editMode)
                pin.IsVisible = Pins[0].IsVisible;

            Pins = new List<Pin>
            {
                pin
            };
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
                    pinModel = await UpdateExistPinAsync(editPinViewModel.Id);
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
                        parameters.Add(ConstantsValue.EDIT_PIN, pinModel);
                    }
                    else
                    {
                        parameters.Add(ConstantsValue.NEW_PIN, pinModel);
                    }

                    await NavigationService.GoBackAsync(parameters);
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Error",
                                                     "Database error.\nPin not saved / updated",
                                                     "Cancel");
                }
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error",
                                                     "Name and coordinates fields must be filled",
                                                     "Cancel");
            }
        }

        private async System.Threading.Tasks.Task<PinModel> UpdateExistPinAsync(int id)
        {
            PinModel pinModel = await _pinService.FindPinModelAsync(p => p.Id == editPinViewModel.Id);

            if (pinModel != null)
            {
                pinModel.Label = LabelPinText;
                pinModel.Description = EditorText;
                // TODO: to change IdCurrentUser
                pinModel.Owner = _authorizeService.IdCurrentUser;
                pinModel.IsEnable = editPinViewModel.IsEnabled;
                int rows = await _pinService.UpdatePinModelAsync(pinModel);

                if(rows <= 0)
                {
                    pinModel = null;
                }
            }

            return pinModel;
        }

        private async System.Threading.Tasks.Task<PinModel> SaveNewPinAsync(Pin pin) 
        {
            pin.Label = LabelPinText;

            PinModel pinModel = pin.PinToPinModel();
            pinModel.Description = EditorText;
            // TODO: to change IdCurrentUser
            pinModel.Owner = _authorizeService.IdCurrentUser;
            pinModel.IsEnable = true;

            int rows = await _pinService.SavePinModelToDatabaseAsync(pinModel);

            if(rows == 0)
            {
                pinModel = null;
            }

            return pinModel;
        }

        #endregion
    }
}