using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Prism.Navigation;
using Prism.Commands;
using GpsNote.Models;
using GpsNote.Services.Localization;

namespace GpsNote.ViewModels.Clock
{
    public class ClockPopupViewModel : ViewModelBase
    {
        public ClockPopupViewModel(INavigationService navigationService, ILocalizationService localizationService) : base(navigationService, localizationService)
        {

        }

        #region -- Public properties --

        private string labelPin;
        public string LabelPin 
        {
            get => labelPin;
            set => SetProperty(ref labelPin, value);
        }

        private DateTime timeZoneDateTime;
        public DateTime TimeZoneDateTime
        {
            get => timeZoneDateTime;
            set => SetProperty(ref timeZoneDateTime, value);
        }

        private string timeZoneID;
        public string TimeZoneID
        {
            get => timeZoneID;
            set => SetProperty(ref timeZoneID, value);
        }

        private string timeString;
        public string TimeString
        {
            get => timeString;
            set => SetProperty(ref timeString, value);
        }


        private bool isTimerAlive;
        public bool IsTimerAlive
        {
            get => isTimerAlive;
            set => SetProperty(ref isTimerAlive, value);
        }

        private DelegateCommand closePopupCommand;
        public DelegateCommand ClosePopupCommand => closePopupCommand ?? new DelegateCommand(OnClosePopupAsync);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            if(parameters.TryGetValue<(Pin, TimeZoneResponse)>(Constants.TUPLE, out (Pin, TimeZoneResponse) tup))
            {
                LabelPin = tup.Item1.Label;
                TimeZoneDateTime = GetDateTime(tup.Item2);
                TimeString = TimeZoneDateTime.ToString("HH:mm");
                TimeZoneID = tup.Item2.TimeZoneID;

                DateTime dt = TimeZoneDateTime;
                IsTimerAlive = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    dt = dt.AddSeconds(1);
                    TimeString = dt.ToString("HH:mm");
                    return IsTimerAlive;
                });
            }
        }

        #endregion

        #region -- Private helpers --

        private DateTime GetDateTime(TimeZoneResponse timeZoneResponse)
        {
            DateTimeOffset offset = new DateTimeOffset(DateTime.Now);
            long timeStamp = offset.ToUnixTimeSeconds();
            timeStamp = timeStamp + (long)timeZoneResponse.DstOffset + (long)timeZoneResponse.RawOffset;
            DateTime dt = UnixTimeStampToDateTime(timeStamp);
            return dt;
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);          
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            
            return dtDateTime;
        }

        private async void OnClosePopupAsync()
        {
            IsTimerAlive = false;
            TimeString = null;
            TimeZoneDateTime = default(DateTime);
            await NavigationService.GoBackAsync();
        }

        #endregion
    }
}