using GpsNote.Models;
using GpsNote.Services.PinService;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Extensions;
using Prism.Services;
using GpsNote.Services.AuthorizeService;

namespace GpsNote.ViewModels
{
    public class AddEditPinViewModel : ViewModelBase
    {

        #region -- Private fields --

        private IPinService _pinService;
        private IPageDialogService _dialogService;
        private IAuthorizeService _authorizeService;

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


        private DelegateCommand<Object> mapTapCommand;
        public DelegateCommand<Object> MapTapCommand => mapTapCommand ?? (new DelegateCommand<Object>(OnMapClickAsync));

        private DelegateCommand saveTapCommand;
        public DelegateCommand SaveTapCommand => saveTapCommand ?? (new DelegateCommand(OnSavePin));

        #endregion


        #region -- Private helpers --

        private async void OnMapClickAsync(Object _position)
        {
            Position position = (Position)_position;
            Pin pin = await _pinService.GetNewPinFromPositionAsync(position);

            LabelPinText = pin.Label;
            LatitudePinText = pin.Position.Latitude.ToString();
            LongitudePinText = pin.Position.Longitude.ToString();

            Pins = new List<Pin>
            {
                pin
            };
        }


        private async void OnSavePin()
        {
            if(string.IsNullOrWhiteSpace(LabelPinText) || 
                string.IsNullOrWhiteSpace(LatitudePinText) ||
                string.IsNullOrWhiteSpace(LongitudePinText))
            {
                await _dialogService.DisplayAlertAsync("Error",
                                                     "Name and coordinates fields must be filled",
                                                     "Cancel");
                return;
            }

            Pin pin;

            try
            {
                pin = await _pinService.GetNewPinFromPositionAsync(
                new Position(Convert.ToDouble(LatitudePinText), Convert.ToDouble(LongitudePinText)));
            }
            catch(Exception ex)
            {
                await _dialogService.DisplayAlertAsync("Error",
                                                     ex.Message,
                                                     "Cancel");
                return;
            }

            pin.Label = LabelPinText;
            PinModelDb pinModelDb = pin.PinToPinModelDb();
            pinModelDb.Description = EditorText;
            pinModelDb.Owner = _authorizeService.IdCurrentUser;
            pinModelDb.IsEnable = true;

            try
            {
                await _pinService.SavePinModelDbToDatabaseAsync(pinModelDb);
            }
            catch(Exception ex)
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

            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("NewPin", pinModelDb);
            await NavigationService.GoBackAsync(parameters);
        }

        #endregion
    }
}