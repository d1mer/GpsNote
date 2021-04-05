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
        public Pin Pin { get; set; }
        public int Owner { get; set; }
    }
}
