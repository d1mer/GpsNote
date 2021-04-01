using System;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Helpers
{
    public class RecordPosition
    {
        public static string WritePositionToString(Position position) => new string($"{position.Latitude}/{position.Longitude}");

        public static Position ReadPositionFromString(string positionStr)
        {
            string[] ar = positionStr.Split('/');
            return new Position(Convert.ToDouble(ar[0]), Convert.ToDouble(ar[1]));
        }
    }
}