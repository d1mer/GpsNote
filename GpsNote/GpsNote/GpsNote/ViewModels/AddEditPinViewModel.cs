using GpsNote.Extensions;
using GpsNote.Models;
using GpsNote.Services.AuthorizeService;
using GpsNote.Services.PinService;
using GpsNote.ViewModels.ExtentedViewModels;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.ViewModels
{
    public class AddEditPinViewModel : ViewModelBase
    {

        #region -- Private fields --

        private IPinService _pinService;
        private IPageDialogService _dialogService;
        private IAuthorizeService _authorizeService;
        private bool editMode = false;
        private PinViewModel editPinViewModel;

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
        public DelegateCommand SaveTapCommand => saveTapCommand ?? (new DelegateCommand(OnSavePin));

        #endregion


        #region -- Override --

        public override void Initialize(INavigationParameters parameters)
        {
            editPinViewModel = parameters.GetValue<PinViewModel>("pin");

            if (editPinViewModel != null)
            {
                LabelPinText = editPinViewModel.Label;
                LatitudePinText = editPinViewModel.Latitude.ToString();
                LongitudePinText = editPinViewModel.Longitude.ToString();
                EditorText = editPinViewModel.Description;

                Pin pin = new Pin
                {
                    Position = new Position(editPinViewModel.Latitude, editPinViewModel.Longitude),
                    Label = editPinViewModel.Label,
                    Address = editPinViewModel.Address
                };

                Pins = new List<Pin>
                {
                    pin
                };

                InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(
                    new CameraPosition(pin.Position, 12));

                editMode = true;
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


        private async void OnSavePin()
        {
            if (string.IsNullOrWhiteSpace(LabelPinText) ||
                string.IsNullOrWhiteSpace(LatitudePinText) ||
                string.IsNullOrWhiteSpace(LongitudePinText))
            {
                await _dialogService.DisplayAlertAsync("Error",
                                                     "Name and coordinates fields must be filled",
                                                     "Cancel");
                return;
            }

            Pin pin;

            if (editMode)
                pin = Pins[0];
            else
            {
                try
                {
                    pin = await _pinService.GetNewPinFromPositionAsync(
                    new Position(Convert.ToDouble(LatitudePinText), Convert.ToDouble(LongitudePinText)));
                }
                catch (Exception ex)
                {
                    await _dialogService.DisplayAlertAsync("Error",
                                                         ex.Message,
                                                         "Cancel");
                    return;
                }
            }

            PinModelDb pinModelDb;

            if (editMode)
            {
                try
                {
                    pinModelDb = await _pinService.FindPinModelDbAsync(p => p.Id == editPinViewModel.Id);

                    if(pinModelDb != null)
                    {
                        pinModelDb.Label = LabelPinText;
                        pinModelDb.Description = EditorText;
                        pinModelDb.Owner = _authorizeService.IdCurrentUser;
                        pinModelDb.IsEnable = editPinViewModel.IsEnabled;
                        await _pinService.UpdatePinModelDbAsync(pinModelDb);
                    }
                }
                catch (Exception ex)
                {
                    await _dialogService.DisplayAlertAsync("Error",
                                                     ex.Message,
                                                     "Cancel");
                    return;
                }
            }
            else
            {
                pin.Label = LabelPinText;
                pinModelDb = pin.PinToPinModelDb();
                pinModelDb.Description = EditorText;
                pinModelDb.Owner = _authorizeService.IdCurrentUser;
                pinModelDb.IsEnable = true;

                try
                {
                    await _pinService.SavePinModelDbToDatabaseAsync(pinModelDb);
                }
                catch (Exception ex)
                {
                    await _dialogService.DisplayAlertAsync("Error",
                                                         ex.Message,
                                                         "Cancel");
                    Pins = new List<Pin>();
                    LabelPinText = string.Empty;
                    LatitudePinText = string.Empty;
                    LongitudePinText = string.Empty;
                    EditorText = string.Empty;
                    return;
                }
            }

            NavigationParameters parameters = new NavigationParameters();
            if(editMode)
                parameters.Add("EditPin", pinModelDb);
            else
                parameters.Add("NewPin", pinModelDb);
            await NavigationService.GoBackAsync(parameters);
        }

        #endregion
    }
}