using GpsNote.Models;
using GpsNote.Services.PinService;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Extensions;
using Prism.Services.Dialogs;
using Prism.Services;

namespace GpsNote.ViewModels
{
    public class AddEditPinViewModel : ViewModelBase
    {
        #region -- Private fields --

        private IPinService _pinService;
        private IPageDialogService _dialogService;

        #endregion


        #region -- Constructor --

        public AddEditPinViewModel(INavigationService navigationService, IPageDialogService dialogService, IPinService pinService) : base(navigationService)
        {
            _pinService = pinService;
            _dialogService = dialogService;

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


        private List<Pin> pins = new List<Pin>();
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
            Pin pin = await _pinService.GetNewPinAsync(position);

            LabelPinText = pin.Label;
            LatitudePinText = pin.Position.Latitude.ToString();
            LongitudePinText = pin.Position.Longitude.ToString();

            //List<Pin> addedPin = new List<Pin>();
            //addedPin.Add(pin);
            //Pins = addedPin;
        }


        private async void OnSavePin()
        {
            if(string.IsNullOrWhiteSpace(LabelPinText) || 
                string.IsNullOrWhiteSpace(LatitudePinText) ||
                string.IsNullOrWhiteSpace(LongitudePinText))
            {
                await _dialogService.DisplayAlertAsync("Error",
                                                     "Must be fill all text fields",
                                                     "Cancel");
                return;
            }

            Pin pin = await _pinService.GetNewPinAsync(
                new Position(Convert.ToDouble(LatitudePinText), Convert.ToDouble(LongitudePinText)));
        }

        #endregion
    }
}