using GpsNote.Helpers;
using GpsNote.Models;
using GpsNote.Services.Localization;
using GpsNote.Services.PinService;
using GpsNote.Services.TimeZone;
using GpsNote.Views.Clock;
using Prism.Commands;
using Prism.Common;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.ViewModels.PinPopup
{
    public class PinInfoViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IPageDialogService _dialogService;
        private Pin pin;

        public PinInfoViewModel(INavigationService navigationService, 
                                ILocalizationService localizationService,
                                IPinService pinService,
                                ITimeZoneService timeZoneService,
                                IPageDialogService dialogService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
            _timeZoneService = timeZoneService;
            _dialogService = dialogService;

            IsVisibleDescription = false;
        }


        #region -- Public properties --

        private bool isVisibleDescription;
        public bool IsVisibleDescription
        {
            get => isVisibleDescription;
            set => SetProperty(ref isVisibleDescription, value);
        }

        private string label;
        public string Label
        {
            get => label;
            set => SetProperty(ref label, value);
        }

        private string longtitude;
        public string Longtitude
        {
            get => longtitude; 
            set => SetProperty(ref longtitude, value); 
        }

        private string latitude;
        public string Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private DelegateCommand popupCloseCommand;
        public DelegateCommand PopupCloseCommand => popupCloseCommand ?? new DelegateCommand(OnPopupCloseAsync);

        private DelegateCommand imageClockTap;
        public DelegateCommand ImageClockTap => imageClockTap ?? new DelegateCommand(OnShowClockAsync);

        #endregion

        #region -- Overrides --

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if(parameters.TryGetValue<Pin>(Constants.DISPLAY_PIN, out Pin pin))
            {
                if(pin != null)
                {
                    PinModel pinModel = await _pinService.FindPinModelAsync(p => p.Label == pin.Label);

                    if(pinModel != null)
                    {
                        if (!string.IsNullOrWhiteSpace(pinModel.Description))
                        {
                            IsVisibleDescription = true;
                            Description = pinModel.Description;
                        }

                        Label = pinModel.Label;
                        Latitude = DoubleOutFormat.Format(pinModel.Latitude);
                        Longtitude = DoubleOutFormat.Format(pinModel.Longitude);
                    }
                }
            }
        }

        #endregion

        #region -- Private helpers --

        private async void OnPopupCloseAsync()
        {
            await NavigationService.GoBackAsync();
        }

        private async void OnShowClockAsync()
        {
            TimeZoneResponse timeZoneResponse = await _timeZoneService.GetTimeZoneAsync(pin.Position);

            if (timeZoneResponse.Status == "OK")
            {
                (Pin, TimeZoneResponse) tup = (pin, timeZoneResponse);

                NavigationParameters parameter = new NavigationParameters
                {
                   {Constants.TUPLE, tup }
                };

                await NavigationService.NavigateAsync(nameof(ClockPopupPage), parameter);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error get TimeZone", timeZoneResponse.Status, "Cancel");
            }
        }
        #endregion
    }
}