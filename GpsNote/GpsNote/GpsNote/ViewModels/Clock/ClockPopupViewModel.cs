using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms.GoogleMaps;
using GpsNote.Models;
using Prism.Navigation;

namespace GpsNote.ViewModels.Clock
{
    public class ClockPopupViewModel : ViewModelBase
    {
        private Pin _pin;
        private TimeZoneResponse _timeZoneResponse;

        public ClockPopupViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        #region -- Public properties --

        private string labelPin;
        public string LabelPin 
        {
            get => labelPin;
            set => SetProperty(ref labelPin, value);
        }

        private string hours;
        public string Hours 
        {
            get => hours;
            set => SetProperty(ref hours, value); 
        }

        private string minutes;
        public string Minutes
        {
            get => minutes;
            set => SetProperty(ref minutes, value);
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

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            if(parameters.TryGetValue<(Pin, TimeZoneResponse)>(Constants.TUPLE, out (Pin, TimeZoneResponse) tup))
            {
                LabelPin = tup.Item1.Label;
                TimeZoneDateTime = GetDateTime(tup.Item2);

                Hours = TimeZoneDateTime.Hour.ToString();
                Minutes = TimeZoneDateTime.Minute.ToString();
                TimeZoneID = tup.Item2.TimeZoneID;
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
            //TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneResponse.TimeZoneID);
            
            //DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            //dtDateTime = dtDateTime.AddSeconds(timeStamp).ToLocalTime();

            //DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(dtDateTime, zone);
            //return dt;
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);          
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            
            return dtDateTime;
        }

        #endregion
    }
}