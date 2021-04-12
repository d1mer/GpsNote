using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Models
{
    public class PinModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Label { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Owner { get; set; }
        public bool IsEnable { get; set; }
    }
}
